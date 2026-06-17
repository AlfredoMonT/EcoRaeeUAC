using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoRaeeUac.Data;
using EcoRaeeUac.Models;

namespace EcoRaeeUac.Controllers;

public class DocentesController : Controller
{
    private readonly ApplicationDbContext _context;

    public DocentesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? searchString, int? page)
    {
        var query = _context.Docentes.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
            query = query.Where(d => d.NombreCompleto.Contains(searchString) || d.Email.Contains(searchString) || d.Especialidad!.Contains(searchString));

        int pageSize = 10;
        return View(await PaginatedList<Docente>.CreateAsync(query.OrderByDescending(d => d.FechaRegistro), page ?? 1, pageSize, searchString));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var docente = await _context.Docentes.FindAsync(id);
        if (docente == null) return NotFound();
        return View(docente);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NombreCompleto,Email,Telefono,Especialidad")] Docente docente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(docente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(docente);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var docente = await _context.Docentes.FindAsync(id);
        if (docente == null) return NotFound();
        return View(docente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,Email,Telefono,Especialidad")] Docente docente)
    {
        if (id != docente.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(docente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Docentes.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(docente);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var docente = await _context.Docentes.FindAsync(id);
        if (docente == null) return NotFound();
        return View(docente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var docente = await _context.Docentes.FindAsync(id);
        if (docente != null)
        {
            _context.Docentes.Remove(docente);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
