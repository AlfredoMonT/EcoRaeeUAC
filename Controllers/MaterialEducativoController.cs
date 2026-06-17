using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcoRaeeUac.Data;
using EcoRaeeUac.Models;

namespace EcoRaeeUac.Controllers;

public class MaterialEducativoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public MaterialEducativoController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index(string? tipo, string? searchString, int? page)
    {
        var materiales = _context.MaterialesEducativos.AsQueryable();

        if (!string.IsNullOrEmpty(tipo))
            materiales = materiales.Where(m => m.Tipo == tipo);

        if (!string.IsNullOrEmpty(searchString))
            materiales = materiales.Where(m => m.Titulo.Contains(searchString) || m.Tipo.Contains(searchString));

        ViewBag.TipoSeleccionado = tipo;

        int pageSize = 12;
        return View(await PaginatedList<MaterialEducativo>.CreateAsync(materiales.OrderByDescending(m => m.FechaPublicacion), page ?? 1, pageSize, searchString));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var material = await _context.MaterialesEducativos.FirstOrDefaultAsync(m => m.Id == id);
        if (material == null) return NotFound();
        return View(material);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Titulo,Tipo,EnlaceUrl,FechaPublicacion")] MaterialEducativo material, IFormFile? archivo)
    {
        if (ModelState.IsValid)
        {
            if (archivo != null && archivo.Length > 0)
            {
                var ruta = await GuardarArchivo(archivo);
                material.RutaArchivo = ruta;
            }

            _context.Add(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(material);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var material = await _context.MaterialesEducativos.FindAsync(id);
        if (material == null) return NotFound();
        return View(material);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MaterialEducativo model, IFormFile? archivo)
    {
        if (id != model.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var material = await _context.MaterialesEducativos.FindAsync(id);
            if (material == null) return NotFound();

            material.Titulo = model.Titulo;
            material.Tipo = model.Tipo;
            material.EnlaceUrl = model.EnlaceUrl;
            material.FechaPublicacion = model.FechaPublicacion;

            if (archivo != null && archivo.Length > 0)
            {
                if (!string.IsNullOrEmpty(material.RutaArchivo))
                    EliminarArchivo(material.RutaArchivo);
                material.RutaArchivo = await GuardarArchivo(archivo);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MaterialesEducativos.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var material = await _context.MaterialesEducativos.FirstOrDefaultAsync(m => m.Id == id);
        if (material == null) return NotFound();
        return View(material);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var material = await _context.MaterialesEducativos.FindAsync(id);
        if (material != null)
        {
            if (!string.IsNullOrEmpty(material.RutaArchivo))
                EliminarArchivo(material.RutaArchivo);

            _context.MaterialesEducativos.Remove(material);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Download(int id)
    {
        var material = await _context.MaterialesEducativos.FindAsync(id);
        if (material == null || string.IsNullOrEmpty(material.RutaArchivo))
            return NotFound();

        var rutaFisica = Path.Combine(_env.WebRootPath, material.RutaArchivo.TrimStart('/'));
        if (!System.IO.File.Exists(rutaFisica))
            return NotFound();

        var ext = Path.GetExtension(material.RutaArchivo);
        var mime = ext?.ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".avi" => "video/x-msvideo",
            _ => "application/octet-stream"
        };

        var nombreDescarga = $"{material.Titulo}{ext}";
        return File(System.IO.File.OpenRead(rutaFisica), mime, nombreDescarga);
    }

    private async Task<string> GuardarArchivo(IFormFile archivo)
    {
        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads", "material");
        Directory.CreateDirectory(uploadsDir);

        var nombreUnico = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
        var rutaCompleta = Path.Combine(uploadsDir, nombreUnico);

        using var stream = new FileStream(rutaCompleta, FileMode.Create);
        await archivo.CopyToAsync(stream);

        return $"/uploads/material/{nombreUnico}";
    }

    private void EliminarArchivo(string rutaRelativa)
    {
        var rutaCompleta = Path.Combine(_env.WebRootPath, rutaRelativa.TrimStart('/'));
        if (System.IO.File.Exists(rutaCompleta))
            System.IO.File.Delete(rutaCompleta);
    }
}
