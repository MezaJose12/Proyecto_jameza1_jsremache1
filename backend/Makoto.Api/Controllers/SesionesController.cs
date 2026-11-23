using Makoto.Api.Data;
using Makoto.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Makoto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SesionesController : ControllerBase
{
    private readonly MakotoDbContext _db;

    public SesionesController(MakotoDbContext db)
    {
        _db = db;
    }

    // DTOs del contrato REST
    public record IniciarSesionRequest(string CodigoParticipante, string Modalidad, bool ConsentimientoAceptado);
    public record RegistrarEventoRequest(string TipoEvento, int? Turno, double? DuracionSegundos, string? DetalleJson);
    public record RegistrarEvaluacionRequest(
        int PetsItem1, int PetsItem2, int PetsItem3, int PetsItem4, int PetsItem5,
        int CalidezPercibida, int NaturalidadVoz,
        string GeneroPercibido, string TonoPercibido, string? ComentarioCualitativo);

    // POST /api/sesiones/iniciar -> Asigna aleatoriamente una de las condiciones con menor cuota
    [HttpPost("iniciar")]
    public async Task<IActionResult> IniciarSesion([FromBody] IniciarSesionRequest req)
    {
        if (!req.ConsentimientoAceptado)
            return BadRequest(new { error = "El consentimiento informado es obligatorio para iniciar." });

        var condiciones = await _db.CondicionesVocales
            .Select(c => new
            {
                Condicion = c,
                TotalCompletadas = c.Sesiones.Count(s => s.Estado == "Completada")
            })
            .ToListAsync();

        var minCuota = condiciones.Min(x => x.TotalCompletadas);
        var candidatas = condiciones.Where(x => x.TotalCompletadas == minCuota).ToList();
        var elegida = candidatas[Random.Shared.Next(candidatas.Count)].Condicion;

        var sesion = new SesionExperimental
        {
            CodigoParticipante = req.CodigoParticipante.Trim(),
            Modalidad = string.IsNullOrWhiteSpace(req.Modalidad) ? "GuionFijo" : req.Modalidad,
            ConsentimientoAceptado = true,
            CondicionVocalId = elegida.Id,
            Estado = "Iniciada"
        };

        _db.SesionesExperimentales.Add(sesion);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(ObtenerSesion), new { id = sesion.Id }, new
        {
            sesionId = sesion.Id,
            estado = sesion.Estado,
            condicionAsignada = new
            {
                elegida.Id,
                elegida.Codigo,
                elegida.Genero,
                elegida.NivelFrecuencia,
                audioUrl = $"/api/audios/{elegida.RutaAudioRelativa}"
            }
        });
    }

    // GET /api/sesiones/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerSesion(Guid id)
    {
        var sesion = await _db.SesionesExperimentales
            .Include(s => s.CondicionVocal)
            .Include(s => s.Evaluacion)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sesion is null) return NotFound(new { error = "Sesión no encontrada." });

        return Ok(new
        {
            sesion.Id,
            sesion.CodigoParticipante,
            sesion.Modalidad,
            sesion.Estado,
            sesion.FechaInicioUtc,
            sesion.FechaFinUtc,
            condicion = sesion.CondicionVocal is null ? null : new
            {
                sesion.CondicionVocal.Codigo,
                sesion.CondicionVocal.Genero,
                sesion.CondicionVocal.NivelFrecuencia
            },
            evaluada = sesion.Evaluacion is not null
        });
    }

    // POST /api/sesiones/{id}/eventos -> Telemetría de audio y turnos
    [HttpPost("{id:guid}/eventos")]
    public async Task<IActionResult> RegistrarEvento(Guid id, [FromBody] RegistrarEventoRequest req)
    {
        var sesion = await _db.SesionesExperimentales.FindAsync(id);
        if (sesion is null) return NotFound(new { error = "Sesión no encontrada." });

        var evento = new EventoTelemetria
        {
            SesionExperimentalId = id,
            TipoEvento = req.TipoEvento,
            Turno = req.Turno,
            DuracionSegundos = req.DuracionSegundos,
            DetalleJson = req.DetalleJson
        };

        if (req.TipoEvento == "AUDIO_START" && sesion.Estado == "Iniciada")
            sesion.Estado = "EnAudio";

        _db.EventosTelemetria.Add(evento);
        await _db.SaveChangesAsync();

        return Ok(new { eventoId = evento.Id, estadoSesion = sesion.Estado });
    }

    // POST /api/sesiones/{id}/evaluacion -> Guarda escala PETS y cierra sesión
    [HttpPost("{id:guid}/evaluacion")]
    public async Task<IActionResult> RegistrarEvaluacion(Guid id, [FromBody] RegistrarEvaluacionRequest req)
    {
        var sesion = await _db.SesionesExperimentales
            .Include(s => s.Evaluacion)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sesion is null) return NotFound(new { error = "Sesión no encontrada." });
        if (sesion.Evaluacion is not null) return Conflict(new { error = "Esta sesión ya tiene una evaluación registrada." });

        var evaluacion = new EvaluacionEmpatia
        {
            SesionExperimentalId = id,
            PetsItem1 = req.PetsItem1,
            PetsItem2 = req.PetsItem2,
            PetsItem3 = req.PetsItem3,
            PetsItem4 = req.PetsItem4,
            PetsItem5 = req.PetsItem5,
            CalidezPercibida = req.CalidezPercibida,
            NaturalidadVoz = req.NaturalidadVoz,
            GeneroPercibido = req.GeneroPercibido,
            TonoPercibido = req.TonoPercibido,
            ComentarioCualitativo = req.ComentarioCualitativo
        };

        sesion.Estado = "Completada";
        sesion.FechaFinUtc = DateTime.UtcNow;

        _db.EvaluacionesEmpatia.Add(evaluacion);
        await _db.SaveChangesAsync();

        var promedioPets = (req.PetsItem1 + req.PetsItem2 + req.PetsItem3 + req.PetsItem4 + req.PetsItem5) / 5.0;
        return Ok(new { evaluacionId = evaluacion.Id, promedioPets, estadoSesion = sesion.Estado });
    }

    // GET /api/sesiones/cuotas -> Resumen balanceo 2x3 para el Dashboard
    [HttpGet("cuotas")]
    public async Task<IActionResult> ObtenerCuotas()
    {
        var cuotas = await _db.CondicionesVocales
            .OrderBy(c => c.Id)
            .Select(c => new
            {
                c.Id,
                c.Codigo,
                c.Genero,
                c.NivelFrecuencia,
                c.FrecuenciaBaseHz,
                totalIniciadas = c.Sesiones.Count(),
                totalCompletadas = c.Sesiones.Count(s => s.Estado == "Completada")
            })
            .ToListAsync();

        return Ok(cuotas);
    }
}