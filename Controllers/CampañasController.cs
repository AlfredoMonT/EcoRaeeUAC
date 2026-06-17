using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoRaeeUac.Data;
using EcoRaeeUac.Models;

namespace EcoRaeeUac.Controllers;

public class CampañasController : Controller
{
    private readonly ApplicationDbContext _context;

    public CampañasController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? searchString, int? page)
    {
        var query = _context.Campañas
            .Include(c => c.Responsable)
            .Include(c => c.Participantes)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
            query = query.Where(c => c.Nombre.Contains(searchString) || c.Lugar.Contains(searchString) || c.Responsable!.NombreCompleto.Contains(searchString));

        query = query.OrderByDescending(c => c.Fecha);
        int pageSize = 10;
        return View(await PaginatedList<CampañaAmbiental>.CreateAsync(query, page ?? 1, pageSize, searchString));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var campaña = await _context.Campañas
            .Include(c => c.Responsable)
            .Include(c => c.Participantes)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (campaña == null) return NotFound();

        return View(campaña);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Docentes = await _context.Docentes.OrderBy(d => d.NombreCompleto).ToListAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Fecha,Lugar,ResponsableId,Descripcion")] CampañaAmbiental campaña)
    {
        if (ModelState.IsValid)
        {
            _context.Add(campaña);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Docentes = await _context.Docentes.OrderBy(d => d.NombreCompleto).ToListAsync();
        return View(campaña);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var campaña = await _context.Campañas.FindAsync(id);
        if (campaña == null) return NotFound();

        ViewBag.Docentes = await _context.Docentes.OrderBy(d => d.NombreCompleto).ToListAsync();
        return View(campaña);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Fecha,Lugar,ResponsableId,Descripcion")] CampañaAmbiental campaña)
    {
        if (id != campaña.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(campaña);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Campañas.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Docentes = await _context.Docentes.OrderBy(d => d.NombreCompleto).ToListAsync();
        return View(campaña);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var campaña = await _context.Campañas
            .Include(c => c.Responsable)
            .Include(c => c.Participantes)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (campaña == null) return NotFound();

        return View(campaña);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var campaña = await _context.Campañas.FindAsync(id);
        if (campaña != null)
        {
            _context.Campañas.Remove(campaña);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
