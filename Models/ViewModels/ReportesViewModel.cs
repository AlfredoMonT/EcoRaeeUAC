namespace EcoRaeeUac.Models.ViewModels;

public class ReportesViewModel
{
    public List<ParticipantesPorCampanaVM> ParticipantesPorCampana { get; set; } = new();
    public List<RaeePorTipoVM> RaeeRecolectadosPorTipo { get; set; } = new();
    public IndicadoresImpactoVM IndicadoresImpacto { get; set; } = new();
    public ReporteRsuVM ReporteRsu { get; set; } = new();
    public int TotalDocentes { get; set; }
}

public class ParticipantesPorCampanaVM
{
    public string Campaña { get; set; } = string.Empty;
    public int TotalParticipantes { get; set; }
    public int Estudiantes { get; set; }
    public int Docentes { get; set; }
    public int CiudadanosBeneficiarios { get; set; }
}

public class RaeePorTipoVM
{
    public string TipoResiduo { get; set; } = string.Empty;
    public double CantidadTotalKg { get; set; }
    public int CantidadRegistros { get; set; }
    public double Co2EvitadoKg { get; set; }
    public double EnergiaAhorradaKwh { get; set; }
    public int ArbolesSalvados { get; set; }
}

public class IndicadoresImpactoVM
{
    public int TotalCampañas { get; set; }
    public int TotalParticipantes { get; set; }
    public int TotalDocentes { get; set; }
    public int TotalMaterialesPublicados { get; set; }
    public double TotalRaeeRecolectadoKg { get; set; }
    public double TotalCo2EvitadoKg { get; set; }
    public double TotalEnergiaAhorradaKwh { get; set; }
    public int TotalArbolesSalvados { get; set; }
    public double PromedioParticipantesPorCampaña { get; set; }
    public double PromedioRaeePorRecoleccionKg { get; set; }
}

public class ReporteRsuVM
{
    public int TotalEstudiantesParticipantes { get; set; }
    public int TotalDocentesParticipantes { get; set; }
    public int TotalCiudadanosBeneficiarios { get; set; }
    public double TotalRaeeRecolectadoKg { get; set; }
    public double TotalCo2EvitadoKg { get; set; }
    public int TotalArbolesSalvados { get; set; }
    public int TotalCampañasRealizadas { get; set; }
    public int TotalMaterialesEducativos { get; set; }
    public string Periodo { get; set; } = DateTime.Now.Year.ToString();
}
