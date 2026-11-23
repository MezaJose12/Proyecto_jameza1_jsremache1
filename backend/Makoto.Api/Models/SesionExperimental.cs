using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makoto.Api.Models;

[Table("sesiones_experimentales")]
public class SesionExperimental
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(64)]
    public string CodigoParticipante { get; set; } = string.Empty; // Hash o código anónimo ESPE

    [Required, MaxLength(20)]
    public string Modalidad { get; set; } = "GuionFijo"; // "GuionFijo" | "Longitudinal"

    public bool ConsentimientoAceptado { get; set; }

    public int CondicionVocalId { get; set; }
    [ForeignKey(nameof(CondicionVocalId))]
    public CondicionVocal? CondicionVocal { get; set; }

    [MaxLength(20)]
    public string Estado { get; set; } = "Iniciada"; // "Iniciada" | "EnAudio" | "Evaluada" | "Completada"

    public DateTime FechaInicioUtc { get; set; } = DateTime.UtcNow;
    public DateTime? FechaFinUtc { get; set; }

    public ICollection<EventoTelemetria> Eventos { get; set; } = new List<EventoTelemetria>();
    public EvaluacionEmpatia? Evaluacion { get; set; }
}