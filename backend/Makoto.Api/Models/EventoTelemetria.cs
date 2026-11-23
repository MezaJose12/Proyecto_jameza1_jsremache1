using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makoto.Api.Models;

[Table("eventos_telemetria")]
public class EventoTelemetria
{
    [Key]
    public long Id { get; set; }

    public Guid SesionExperimentalId { get; set; }
    [ForeignKey(nameof(SesionExperimentalId))]
    public SesionExperimental? SesionExperimental { get; set; }

    [Required, MaxLength(50)]
    public string TipoEvento { get; set; } = string.Empty; // "AUDIO_START", "AUDIO_END", "REPLAY", "INTERACCION_TURNO"

    public int? Turno { get; set; }
    public double? DuracionSegundos { get; set; }

    [MaxLength(500)]
    public string? DetalleJson { get; set; }

    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}