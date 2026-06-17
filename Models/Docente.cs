using System.ComponentModel.DataAnnotations;

namespace EcoRaeeUac.Models;

public class Docente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(200)]
    [Display(Name = "Nombre Completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de email inválido.")]
    [StringLength(200)]
    [Display(Name = "Correo Electrónico")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Teléfono")]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [Display(Name = "Especialidad")]
    [StringLength(200)]
    public string? Especialidad { get; set; }

    [Display(Name = "Fecha de Registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}
