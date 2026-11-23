using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makoto.Api.Models;

[Table("evaluaciones_empatia")]
public class EvaluacionEmpatia
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SesionExperimentalId { get; set; }
    [ForeignKey(nameof(SesionExperimentalId))]
    public SesionExperimental? SesionExperimental { get; set; }

    // Ítems Likert (1 a 7) de Escala PETS (Perceived Empathy of Technology Scale)
    [Range(1, 7)] public int PetsItem1 { get; set; }
    [Range(1, 7)] public int PetsItem2 { get; set; }
    [Range(1, 7)] public int PetsItem3 { get; set; }
    [Range(1, 7)] public int PetsItem4 { get; set; }
    [Range(1, 7)] public int PetsItem5 { get; set; }

    // Percepción de Calidez y Naturalidad (1 a 7)
    [Range(1, 7)] public int CalidezPercibida { get; set; }
    [Range(1, 7)] public int NaturalidadVoz { get; set; }

    // Chequeo de manipulación (qué género/tono percibió el participante)
    [Required, MaxLength(30)]
    public string GeneroPercibido { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string TonoPercibido { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? ComentarioCualitativo { get; set; }

    public DateTime FechaRegistroUtc { get; set; } = DateTime.UtcNow;
}