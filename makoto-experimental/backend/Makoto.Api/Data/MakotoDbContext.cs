using Microsoft.EntityFrameworkCore;

namespace Makoto.Api.Data;

public class MakotoDbContext(DbContextOptions<MakotoDbContext> options) : DbContext(options)
{
    public DbSet<SesionExperimental> SesionesExperimentales => Set<SesionExperimental>();
}

public class SesionExperimental
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CodigoAnonimo { get; set; } = string.Empty;
    public string GeneroVocal { get; set; } = string.Empty;   // Femenino | Masculino
    public string NivelF0 { get; set; } = string.Empty;       // Baja | Media | Alta
    public DateTime FechaCreacionUtc { get; set; } = DateTime.UtcNow;
}