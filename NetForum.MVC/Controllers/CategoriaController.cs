using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.MVC.Models;
using NetForum.MVC.Security;

namespace NetForum.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class CategoriaController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de la abstracción
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var categorias = string.IsNullOrWhiteSpace(buscar)
            ? await _categoriaService.ObtenerCategoriasAsync()
            : await _categoriaService.BuscarCategoriasAsync(buscar);

        ViewBag.Buscar = buscar;

        return View(categorias);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CategoriaViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoriaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var categoria = new Categoria
        {
            Nombre = vm.Nombre,
            Descripcion = vm.Descripcion,
            EstadoId = 1
        };

        await _categoriaService.GuardarAsync(categoria);

        TempData["Success"] = "Categoría creada correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var categoria = await _categoriaService.ObtenerPorIdAsync(id);

        if (categoria is null)
        {
            return NotFound();
        }

        var vm = new CategoriaViewModel
        {
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion
        };

        ViewBag.CategoriaId = categoria.CategoriaId;

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoriaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.CategoriaId = id;
            return View(vm);
        }

        var categoria = await _categoriaService.ObtenerPorIdAsync(id);

        if (categoria is null)
        {
            return NotFound();
        }

        categoria.Nombre = vm.Nombre;
        categoria.Descripcion = vm.Descripcion;

        await _categoriaService.ActualizarAsync(categoria);

        TempData["Success"] = "Categoría actualizada correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var categoria = await _categoriaService.ObtenerPorIdAsync(id);

        if (categoria is null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _categoriaService.EliminarAsync(id);

            TempData["Success"] =
                "Categoría eliminada correctamente.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }
        catch (KeyNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}