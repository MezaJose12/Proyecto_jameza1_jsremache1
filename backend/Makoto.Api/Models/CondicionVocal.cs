using Makoto.Api.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Makoto.Api.Models;

[Table("condiciones_vocales")]
public class CondicionVocal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; } // 1 al 6

    [Required, MaxLength(10)]
    public string Codigo { get; set; } = string.Empty; // Ej: "F_GRAVE", "F_NEUTRA", "F_AGUDA", "M_GRAVE", etc.

    [Required, MaxLength(20)]
    public string Genero { get; set; } = string.Empty; // "Femenino" | "Masculino"

    [Required, MaxLength(20)]
    public string NivelFrecuencia { get; set; } = string.Empty; // "Grave" | "Neutra" | "Aguda"

    public double FrecuenciaBaseHz { get; set; } // Ej: 165.0, 210.0, 255.0

    [Required, MaxLength(255)]
    public string RutaAudioRelativa { get; set; } = string.Empty; // Ej: "f_neutra_guion1.wav"

    public ICollection<SesionExperimental> Sesiones { get; set; } = new List<SesionExperimental>();
}