using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoRaeeUac.Data;
using EcoRaeeUac.Models;

namespace EcoRaeeUac.Controllers;

public class ParticipantesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ParticipantesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? campañaId, string? searchString, int? page)
    {
        var participantes = _context.Participantes
            .Include(p => p.CampañaAmbiental)
            .Include(p => p.Docente)
            .AsQueryable();

        if (campañaId.HasValue)
            participantes = participantes.Where(p => p.CampañaAmbientalId == campañaId.Value);

        if (!string.IsNullOrEmpty(searchString))
            participantes = participantes.Where(p => p.NombreCompleto.Contains(searchString) || p.DocumentoIdentidad.Contains(searchString) || p.Rol.Contains(searchString));

        ViewBag.Campañas = await _context.Campañas.ToListAsync();
        ViewBag.CampañaSeleccionada = campañaId;

        int pageSize = 10;
        return View(await PaginatedList<Participante>.CreateAsync(participantes.OrderByDescending(p => p.Id), page ?? 1, pageSize, searchString));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var participante = await _context.Participantes
            .Include(p => p.CampañaAmbiental)
            .Include(p => p.Docente)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (participante == null) return NotFound();

        return View(participante);
    }

    public async Task<IActionResult> Create(int? campañaId)
    {
        ViewBag.Campañas = await _context.Campañas.OrderByDescending(c => c.Fecha).ToListAsync();
        ViewBag.Docentes = await _context.Docentes.OrderBy(d => d.NombreCompleto).ToListAsync();
        ViewBag.CampañaId = campañaId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NombreCompleto,Rol,DocumentoIdentidad,CampañaAmbientalId,DocenteId")] Participante participante)
    {
        if (ModelState.IsValid)
        {
            _context.Add(participante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Campañas = await _context.Campañas.OrderByDescending(c => c.Fecha).ToListAsync();
        ViewBag.Docentes = await _context.Docentes.OrderBy(d => d.NombreCompleto).ToListAsync();
        return View(participante);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var participante = await _context.Participantes.FindAsync(id);
        if (participante == null) return NotFound();

        ViewBag.Campañas = await _context.Campañas.OrderByDescending(c => c.Fecha).ToListAsync();
        ViewBag.Docentes = await _context.Docentes.OrderBy(d => d.NombreCompleto).ToListAsync();
        return View(participante);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,Rol,DocumentoIdentidad,CampañaAmbientalId,DocenteId")] Participante participante)
    {
        if (id != participante.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(participante);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Participantes.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Campañas = await _context.Campañas.OrderByDescending(c => c.Fecha).ToListAsync();
        ViewBag.Docentes = await _context.Docentes.OrderBy(d => d.NombreCompleto).ToListAsync();
        return View(participante);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var participante = await _context.Participantes
            .Include(p => p.CampañaAmbiental)
            .Include(p => p.Docente)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (participante == null) return NotFound();

        return View(participante);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var participante = await _context.Participantes.FindAsync(id);
        if (participante != null)
        {
            _context.Participantes.Remove(participante);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
