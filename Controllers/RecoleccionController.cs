using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoRaeeUac.Data;
using EcoRaeeUac.Models;

namespace EcoRaeeUac.Controllers;

public class RecoleccionController : Controller
{
    private readonly ApplicationDbContext _context;

    public RecoleccionController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? tipoResiduo, string? searchString, int? page)
    {
        var recolecciones = _context.Recolecciones.AsQueryable();

        if (!string.IsNullOrEmpty(tipoResiduo))
            recolecciones = recolecciones.Where(r => r.TipoResiduo == tipoResiduo);

        if (!string.IsNullOrEmpty(searchString))
            recolecciones = recolecciones.Where(r => r.TipoResiduo.Contains(searchString) || r.LugarRecoleccion.Contains(searchString));

        ViewBag.TipoSeleccionado = tipoResiduo;

        // Compute totals BEFORE pagination
        var totalKg = await recolecciones.SumAsync(r => r.Cantidad);
        var totalCo2 = await recolecciones.SumAsync(r => r.Co2EvitadoKg);
        var totalArboles = await recolecciones.SumAsync(r => r.ArbolesSalvados);
        var totalEnergia = await recolecciones.SumAsync(r => r.EnergiaAhorradaKwh);

        ViewBag.TotalKg = totalKg;
        ViewBag.TotalCo2 = totalCo2;
        ViewBag.TotalArboles = totalArboles;
        ViewBag.TotalEnergia = totalEnergia;

        int pageSize = 10;
        return View(await PaginatedList<RecoleccionRaee>.CreateAsync(recolecciones.OrderByDescending(r => r.Fecha), page ?? 1, pageSize, searchString));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var recoleccion = await _context.Recolecciones.FirstOrDefaultAsync(m => m.Id == id);
        if (recoleccion == null) return NotFound();
        return View(recoleccion);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TipoResiduo,Cantidad,LugarRecoleccion,Fecha")] RecoleccionRaee recoleccion)
    {
        if (ModelState.IsValid)
        {
            recoleccion.CalcularIndicadores();
            _context.Add(recoleccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(recoleccion);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var recoleccion = await _context.Recolecciones.FindAsync(id);
        if (recoleccion == null) return NotFound();
        return View(recoleccion);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,TipoResiduo,Cantidad,LugarRecoleccion,Fecha")] RecoleccionRaee recoleccion)
    {
        if (id != recoleccion.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                recoleccion.CalcularIndicadores();
                _context.Update(recoleccion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Recolecciones.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(recoleccion);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var recoleccion = await _context.Recolecciones.FirstOrDefaultAsync(m => m.Id == id);
        if (recoleccion == null) return NotFound();
        return View(recoleccion);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var recoleccion = await _context.Recolecciones.FindAsync(id);
        if (recoleccion != null)
        {
            _context.Recolecciones.Remove(recoleccion);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
