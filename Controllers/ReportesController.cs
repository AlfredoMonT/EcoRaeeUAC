using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoRaeeUac.Data;
using EcoRaeeUac.Models.ViewModels;
using ClosedXML.Excel;

namespace EcoRaeeUac.Controllers;

public class ReportesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new ReportesViewModel();
        await CargarDatos(model);
        return View(model);
    }

    public async Task<IActionResult> ExportarExcel()
    {
        var model = new ReportesViewModel();
        await CargarDatos(model);

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Reporte General");

        ws.Cell(1, 1).Value = "Reporte General EcoRAEE UAC";
        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 14;
        ws.Range(1, 1, 1, 6).Merge();

        ws.Cell(3, 1).Value = "INDICADORES DE IMPACTO";
        ws.Cell(3, 1).Style.Font.Bold = true;
        ws.Cell(4, 1).Value = "Total Campañas";
        ws.Cell(4, 2).Value = model.IndicadoresImpacto.TotalCampañas;
        ws.Cell(5, 1).Value = "Total Participantes";
        ws.Cell(5, 2).Value = model.IndicadoresImpacto.TotalParticipantes;
        ws.Cell(6, 1).Value = "Total Docentes";
        ws.Cell(6, 2).Value = model.IndicadoresImpacto.TotalDocentes;
        ws.Cell(7, 1).Value = "RAEE Recolectado (Kg)";
        ws.Cell(7, 2).Value = model.IndicadoresImpacto.TotalRaeeRecolectadoKg;
        ws.Cell(8, 1).Value = "CO₂ Evitado (Kg)";
        ws.Cell(8, 2).Value = model.IndicadoresImpacto.TotalCo2EvitadoKg;
        ws.Cell(9, 1).Value = "Energía Ahorrada (kWh)";
        ws.Cell(9, 2).Value = model.IndicadoresImpacto.TotalEnergiaAhorradaKwh;
        ws.Cell(10, 1).Value = "Árboles Salvados";
        ws.Cell(10, 2).Value = model.IndicadoresImpacto.TotalArbolesSalvados;
        ws.Cell(11, 1).Value = "Material Educativo";
        ws.Cell(11, 2).Value = model.IndicadoresImpacto.TotalMaterialesPublicados;

        int fila = 13;
        ws.Cell(fila, 1).Value = "PARTICIPANTES POR CAMPAÑA";
        ws.Cell(fila, 1).Style.Font.Bold = true;
        fila++;
        ws.Cell(fila, 1).Value = "Campaña";
        ws.Cell(fila, 2).Value = "Total";
        ws.Cell(fila, 3).Value = "Estudiantes";
        ws.Cell(fila, 4).Value = "Docentes";
        ws.Cell(fila, 5).Value = "Ciudadanos";
        ws.Range(fila, 1, fila, 5).Style.Font.Bold = true;
        fila++;

        foreach (var item in model.ParticipantesPorCampana)
        {
            ws.Cell(fila, 1).Value = item.Campaña;
            ws.Cell(fila, 2).Value = item.TotalParticipantes;
            ws.Cell(fila, 3).Value = item.Estudiantes;
            ws.Cell(fila, 4).Value = item.Docentes;
            ws.Cell(fila, 5).Value = item.CiudadanosBeneficiarios;
            fila++;
        }

        fila += 2;
        ws.Cell(fila, 1).Value = "RAEE RECOLECTADO POR TIPO";
        ws.Cell(fila, 1).Style.Font.Bold = true;
        fila++;
        ws.Cell(fila, 1).Value = "Tipo";
        ws.Cell(fila, 2).Value = "Kg";
        ws.Cell(fila, 3).Value = "Registros";
        ws.Cell(fila, 4).Value = "CO₂ Evitado (Kg)";
        ws.Cell(fila, 5).Value = "Energía (kWh)";
        ws.Cell(fila, 6).Value = "Árboles";
        ws.Range(fila, 1, fila, 6).Style.Font.Bold = true;
        fila++;

        foreach (var item in model.RaeeRecolectadosPorTipo)
        {
            ws.Cell(fila, 1).Value = item.TipoResiduo;
            ws.Cell(fila, 2).Value = item.CantidadTotalKg;
            ws.Cell(fila, 3).Value = item.CantidadRegistros;
            ws.Cell(fila, 4).Value = item.Co2EvitadoKg;
            ws.Cell(fila, 5).Value = item.EnergiaAhorradaKwh;
            ws.Cell(fila, 6).Value = item.ArbolesSalvados;
            fila++;
        }

        fila += 2;
        ws.Cell(fila, 1).Value = "REPORTE RSU";
        ws.Cell(fila, 1).Style.Font.Bold = true;
        fila++;
        ws.Cell(fila, 1).Value = "Estudiantes";
        ws.Cell(fila, 2).Value = model.ReporteRsu.TotalEstudiantesParticipantes;
        fila++;
        ws.Cell(fila, 1).Value = "Docentes";
        ws.Cell(fila, 2).Value = model.ReporteRsu.TotalDocentesParticipantes;
        fila++;
        ws.Cell(fila, 1).Value = "Ciudadanos Beneficiarios";
        ws.Cell(fila, 2).Value = model.ReporteRsu.TotalCiudadanosBeneficiarios;
        fila++;
        ws.Cell(fila, 1).Value = "RAEE Recolectado (Kg)";
        ws.Cell(fila, 2).Value = model.ReporteRsu.TotalRaeeRecolectadoKg;
        fila++;
        ws.Cell(fila, 1).Value = "CO₂ Evitado (Kg)";
        ws.Cell(fila, 2).Value = model.ReporteRsu.TotalCo2EvitadoKg;
        fila++;
        ws.Cell(fila, 1).Value = "Árboles Salvados";
        ws.Cell(fila, 2).Value = model.ReporteRsu.TotalArbolesSalvados;
        fila++;
        ws.Cell(fila, 1).Value = "Campañas Realizadas";
        ws.Cell(fila, 2).Value = model.ReporteRsu.TotalCampañasRealizadas;

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var contenido = stream.ToArray();

        return File(contenido, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Reporte_EcoRAEE_{DateTime.Now:yyyyMMdd}.xlsx");
    }

    private async Task CargarDatos(ReportesViewModel model)
    {
        await CargarParticipantesPorCampana(model);
        await CargarRaeePorTipo(model);
        await CargarIndicadoresImpacto(model);
        await CargarReporteRsu(model);
        model.TotalDocentes = await _context.Docentes.CountAsync();
    }

    private async Task CargarParticipantesPorCampana(ReportesViewModel model)
    {
        var query = from p in _context.Participantes
                    join c in _context.Campañas on p.CampañaAmbientalId equals c.Id
                    group p by new { c.Id, c.Nombre } into g
                    select new ParticipantesPorCampanaVM
                    {
                        Campaña = g.Key.Nombre,
                        TotalParticipantes = g.Count(),
                        Estudiantes = g.Count(p => p.Rol == "Estudiante"),
                        Docentes = g.Count(p => p.Rol == "Docente"),
                        CiudadanosBeneficiarios = g.Count(p => p.Rol == "Ciudadano Beneficiario")
                    };

        model.ParticipantesPorCampana = await query.OrderByDescending(x => x.TotalParticipantes).ToListAsync();
    }

    private async Task CargarRaeePorTipo(ReportesViewModel model)
    {
        var query = from r in _context.Recolecciones
                    group r by r.TipoResiduo into g
                    select new RaeePorTipoVM
                    {
                        TipoResiduo = g.Key,
                        CantidadTotalKg = g.Sum(r => r.Cantidad),
                        CantidadRegistros = g.Count(),
                        Co2EvitadoKg = g.Sum(r => r.Co2EvitadoKg),
                        EnergiaAhorradaKwh = g.Sum(r => r.EnergiaAhorradaKwh),
                        ArbolesSalvados = g.Sum(r => r.ArbolesSalvados)
                    };

        model.RaeeRecolectadosPorTipo = await query.OrderByDescending(x => x.CantidadTotalKg).ToListAsync();
    }

    private async Task CargarIndicadoresImpacto(ReportesViewModel model)
    {
        var totalCampañas = await _context.Campañas.CountAsync();
        var totalParticipantes = await _context.Participantes.CountAsync();
        var totalDocentes = await _context.Docentes.CountAsync();
        var totalMateriales = await _context.MaterialesEducativos.CountAsync();
        var recolecciones = await _context.Recolecciones.ToListAsync();
        var totalRaeeKg = recolecciones.Sum(r => r.Cantidad);
        var totalCo2 = recolecciones.Sum(r => r.Co2EvitadoKg);
        var totalEnergia = recolecciones.Sum(r => r.EnergiaAhorradaKwh);
        var totalArboles = recolecciones.Sum(r => r.ArbolesSalvados);

        model.IndicadoresImpacto = new IndicadoresImpactoVM
        {
            TotalCampañas = totalCampañas,
            TotalParticipantes = totalParticipantes,
            TotalDocentes = totalDocentes,
            TotalMaterialesPublicados = totalMateriales,
            TotalRaeeRecolectadoKg = totalRaeeKg,
            TotalCo2EvitadoKg = totalCo2,
            TotalEnergiaAhorradaKwh = totalEnergia,
            TotalArbolesSalvados = totalArboles,
            PromedioParticipantesPorCampaña = totalCampañas > 0
                ? Math.Round((double)totalParticipantes / totalCampañas, 1) : 0,
            PromedioRaeePorRecoleccionKg = recolecciones.Count > 0
                ? Math.Round(totalRaeeKg / recolecciones.Count, 2) : 0
        };
    }

    private async Task CargarReporteRsu(ReportesViewModel model)
    {
        var totalEstudiantes = await _context.Participantes.CountAsync(p => p.Rol == "Estudiante");
        var totalDocentes = await _context.Participantes.CountAsync(p => p.Rol == "Docente");
        var totalCiudadanos = await _context.Participantes.CountAsync(p => p.Rol == "Ciudadano Beneficiario");
        var recolecciones = await _context.Recolecciones.ToListAsync();
        var totalCo2 = recolecciones.Sum(r => r.Co2EvitadoKg);
        var totalArboles = recolecciones.Sum(r => r.ArbolesSalvados);
        var totalRaeeKg = recolecciones.Sum(r => r.Cantidad);
        var totalCampañas = await _context.Campañas.CountAsync();
        var totalMateriales = await _context.MaterialesEducativos.CountAsync();

        model.ReporteRsu = new ReporteRsuVM
        {
            TotalEstudiantesParticipantes = totalEstudiantes,
            TotalDocentesParticipantes = totalDocentes,
            TotalCiudadanosBeneficiarios = totalCiudadanos,
            TotalRaeeRecolectadoKg = totalRaeeKg,
            TotalCo2EvitadoKg = totalCo2,
            TotalArbolesSalvados = totalArboles,
            TotalCampañasRealizadas = totalCampañas,
            TotalMaterialesEducativos = totalMateriales
        };
    }
}
