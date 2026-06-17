using System.ComponentModel.DataAnnotations;

namespace EcoRaeeUac.Models;

public class Participante
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
    [Display(Name = "Nombre Completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El rol es obligatorio.")]
    [StringLength(50)]
    [Display(Name = "Rol")]
    public string Rol { get; set; } = string.Empty;

    [Required(ErrorMessage = "El documento de identidad es obligatorio.")]
    [StringLength(20, ErrorMessage = "El documento no puede superar los 20 caracteres.")]
    [Display(Name = "Documento de Identidad")]
    public string DocumentoIdentidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La campaña es obligatoria.")]
    [Display(Name = "Campaña")]
    public int CampañaAmbientalId { get; set; }

    [Display(Name = "Campaña")]
    public CampañaAmbiental CampañaAmbiental { get; set; } = null!;

    [Display(Name = "Docente Vinculado")]
    public int? DocenteId { get; set; }

    [Display(Name = "Docente Vinculado")]
    public Docente? Docente { get; set; }
}
