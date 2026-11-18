using Makoto.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace Makoto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController(MakotoDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var dbConectada = await db.Database.CanConnectAsync();
        var volumenAudiosListo = Directory.Exists("/app/datos/audios");

        return Ok(new
        {
            proyecto = "Makoto - Diseño Factorial 2x3",
            estado = dbConectada ? "OK" : "ERROR_DB",
            puertoApi = 8080,
            puertoPostgres = 5432,
            postgresqlConectado = dbConectada,
            volumenAudiosListo,
            timestampUtc = DateTime.UtcNow
        });
    }
}