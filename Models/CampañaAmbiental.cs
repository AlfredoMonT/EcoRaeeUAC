using System.ComponentModel.DataAnnotations;

namespace EcoRaeeUac.Models;

public class CampañaAmbiental
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la campaña es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
    [Display(Name = "Nombre de la Campaña")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha del Evento")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "El lugar es obligatorio.")]
    [StringLength(200, ErrorMessage = "El lugar no puede superar los 200 caracteres.")]
    [Display(Name = "Lugar")]
    public string Lugar { get; set; } = string.Empty;

    [Required(ErrorMessage = "El responsable es obligatorio.")]
    [Display(Name = "Responsable")]
    public int ResponsableId { get; set; }

    [Display(Name = "Responsable")]
    public Docente Responsable { get; set; } = null!;

    [DataType(DataType.MultilineText)]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    public ICollection<Participante> Participantes { get; set; } = new List<Participante>();
}
