using Makoto.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Makoto.Api.Data;

public class MakotoDbContext : DbContext
{
    public MakotoDbContext(DbContextOptions<MakotoDbContext> options) : base(options) { }

    public DbSet<CondicionVocal> CondicionesVocales => Set<CondicionVocal>();
    public DbSet<SesionExperimental> SesionesExperimentales => Set<SesionExperimental>();
    public DbSet<EventoTelemetria> EventosTelemetria => Set<EventoTelemetria>();
    public DbSet<EvaluacionEmpatia> EvaluacionesEmpatia => Set<EvaluacionEmpatia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CondicionVocal>()
            .HasIndex(c => c.Codigo)
            .IsUnique();

        modelBuilder.Entity<SesionExperimental>()
            .HasOne(s => s.Evaluacion)
            .WithOne(e => e.SesionExperimental)
            .HasForeignKey<EvaluacionEmpatia>(e => e.SesionExperimentalId)
            .OnDelete(DeleteBehavior.Cascade);

        // Semilla inicial de la matriz factorial 2x3
        modelBuilder.Entity<CondicionVocal>().HasData(
            new CondicionVocal { Id = 1, Codigo = "F_GRAVE", Genero = "Femenino", NivelFrecuencia = "Grave", FrecuenciaBaseHz = 165.0, RutaAudioRelativa = "f_grave.wav" },
            new CondicionVocal { Id = 2, Codigo = "F_NEUTRA", Genero = "Femenino", NivelFrecuencia = "Neutra", FrecuenciaBaseHz = 210.0, RutaAudioRelativa = "f_neutra.wav" },
            new CondicionVocal { Id = 3, Codigo = "F_AGUDA", Genero = "Femenino", NivelFrecuencia = "Aguda", FrecuenciaBaseHz = 255.0, RutaAudioRelativa = "f_aguda.wav" },
            new CondicionVocal { Id = 4, Codigo = "M_GRAVE", Genero = "Masculino", NivelFrecuencia = "Grave", FrecuenciaBaseHz = 100.0, RutaAudioRelativa = "m_grave.wav" },
            new CondicionVocal { Id = 5, Codigo = "M_NEUTRA", Genero = "Masculino", NivelFrecuencia = "Neutra", FrecuenciaBaseHz = 130.0, RutaAudioRelativa = "m_neutra.wav" },
            new CondicionVocal { Id = 6, Codigo = "M_AGUDA", Genero = "Masculino", NivelFrecuencia = "Aguda", FrecuenciaBaseHz = 165.0, RutaAudioRelativa = "m_aguda.wav" }
        );
    }
}