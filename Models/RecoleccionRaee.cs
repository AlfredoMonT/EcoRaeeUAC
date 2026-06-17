using System.ComponentModel.DataAnnotations;

namespace EcoRaeeUac.Models;

public class RecoleccionRaee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El tipo de residuo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El tipo de residuo no puede superar los 100 caracteres.")]
    [Display(Name = "Tipo de Residuo")]
    public string TipoResiduo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(0.01, 999999.99, ErrorMessage = "La cantidad debe estar entre 0.01 y 999,999.99 Kg.")]
    [Display(Name = "Cantidad (Kg)")]
    public double Cantidad { get; set; }

    [Required(ErrorMessage = "El lugar de recolección es obligatorio.")]
    [StringLength(200, ErrorMessage = "El lugar no puede superar los 200 caracteres.")]
    [Display(Name = "Lugar de Recolección")]
    public string LugarRecoleccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de Recolección")]
    public DateTime Fecha { get; set; }

    [Display(Name = "CO₂ Evitado (Kg)")]
    public double Co2EvitadoKg { get; set; }

    [Display(Name = "Material Recuperado (Kg)")]
    public double MaterialRecuperadoKg { get; set; }

    [Display(Name = "Árboles Salvados")]
    public int ArbolesSalvados { get; set; }

    [Display(Name = "Energía Ahorrada (kWh)")]
    public double EnergiaAhorradaKwh { get; set; }

    public void CalcularIndicadores()
    {
        Co2EvitadoKg = Math.Round(Cantidad * 0.85, 2);
        MaterialRecuperadoKg = Math.Round(Cantidad * 0.93, 2);
        ArbolesSalvados = (int)(Cantidad * 0.012);
        EnergiaAhorradaKwh = Math.Round(Cantidad * 2.5, 2);
    }
}
