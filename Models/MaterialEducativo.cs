using System.ComponentModel.DataAnnotations;

namespace EcoRaeeUac.Models;

public class MaterialEducativo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(200, ErrorMessage = "El título no puede superar los 200 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de material es obligatorio.")]
    [StringLength(30)]
    [Display(Name = "Tipo de Material")]
    public string Tipo { get; set; } = string.Empty;

    [Display(Name = "Archivo Subido")]
    public string? RutaArchivo { get; set; }

    [Url(ErrorMessage = "Debe ingresar una URL válida.")]
    [Display(Name = "URL Externa (opcional)")]
    public string? EnlaceUrl { get; set; }

    [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Publicación")]
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;

    public string ObtenerTipoContenido()
    {
        if (!string.IsNullOrEmpty(RutaArchivo))
        {
            var ext = Path.GetExtension(RutaArchivo)?.ToLower();
            return ext switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" => "imagen",
                ".mp4" or ".webm" or ".avi" => "video",
                ".pdf" => "pdf",
                _ => "archivo"
            };
        }
        if (!string.IsNullOrEmpty(EnlaceUrl))
            return Tipo.ToLower();
        return "archivo";
    }
}
