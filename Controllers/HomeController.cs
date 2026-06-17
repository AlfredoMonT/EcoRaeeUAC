using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoRaeeUac.Data;
using EcoRaeeUac.Models;

namespace EcoRaeeUac.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalCampañas = await _context.Campañas.CountAsync();
        ViewBag.TotalParticipantes = await _context.Participantes.CountAsync();
        ViewBag.TotalDocentes = await _context.Docentes.CountAsync();
        ViewBag.TotalRecolecciones = await _context.Recolecciones.CountAsync();
        ViewBag.TotalMateriales = await _context.MaterialesEducativos.CountAsync();
        ViewBag.TotalRaeeKg = await _context.Recolecciones.SumAsync(r => r.Cantidad);
        ViewBag.TotalCo2Kg = await _context.Recolecciones.SumAsync(r => r.Co2EvitadoKg);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
