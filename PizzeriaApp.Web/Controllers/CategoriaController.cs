using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class CategoriaController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public CategoriaController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categorias = await _context.Categorias
            .Include(c => c.Productos)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        return View(categorias);
    }

    public IActionResult Create()
    {
        return View(new Categoria());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Categoria categoria)
    {
        ModelState.Remove(nameof(categoria.Productos));

        categoria.Nombre = (categoria.Nombre ?? string.Empty).Trim();
        categoria.Descripcion = (categoria.Descripcion ?? string.Empty).Trim();

        ValidarCategoria(categoria);

        if (await _context.Categorias.AnyAsync(c =>
            c.Nombre.ToLower() == categoria.Nombre.ToLower()))
        {
            ModelState.AddModelError(nameof(categoria.Nombre),
                "Ya existe una categoría con ese nombre.");
        }

        if (!ModelState.IsValid)
            return View(categoria);

        categoria.Activo = true;

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Categoría creada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
            return BadRequest();

        ModelState.Remove(nameof(categoria.Productos));

        var existente = await _context.Categorias.FindAsync(id);

        if (existente == null)
            return NotFound();

        categoria.Nombre = (categoria.Nombre ?? string.Empty).Trim();
        categoria.Descripcion = (categoria.Descripcion ?? string.Empty).Trim();

        ValidarCategoria(categoria);

        if (await _context.Categorias.AnyAsync(c =>
            c.CategoriaId != id &&
            c.Nombre.ToLower() == categoria.Nombre.ToLower()))
        {
            ModelState.AddModelError(nameof(categoria.Nombre),
                "Ya existe otra categoría con ese nombre.");
        }

        if (!ModelState.IsValid)
            return View(categoria);

        existente.Nombre = categoria.Nombre;
        existente.Descripcion = categoria.Descripcion;
        existente.Activo = categoria.Activo;

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Categoría actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActivo(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.CategoriaId == id);

        if (categoria == null)
        {
            TempData["Mensaje"] = "La categoría no existe.";
            return RedirectToAction(nameof(Index));
        }

        if (categoria.Activo && categoria.Productos.Any(p => p.Activo))
        {
            TempData["Mensaje"] =
                "No se puede desactivar esta categoría porque tiene productos activos.";
            return RedirectToAction(nameof(Index));
        }

        categoria.Activo = !categoria.Activo;

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = categoria.Activo
            ? "Categoría activada correctamente."
            : "Categoría desactivada correctamente.";

        return RedirectToAction(nameof(Index));
    }

    private void ValidarCategoria(Categoria categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria.Nombre))
            ModelState.AddModelError(nameof(categoria.Nombre),
                "El nombre es obligatorio.");
        else if (categoria.Nombre.Length > 50)
            ModelState.AddModelError(nameof(categoria.Nombre),
                "El nombre no puede superar los 50 caracteres.");

        if (string.IsNullOrWhiteSpace(categoria.Descripcion))
            ModelState.AddModelError(nameof(categoria.Descripcion),
                "La descripción es obligatoria.");
        else if (categoria.Descripcion.Length > 300)
            ModelState.AddModelError(nameof(categoria.Descripcion),
                "La descripción no puede superar los 300 caracteres.");
    }
}
