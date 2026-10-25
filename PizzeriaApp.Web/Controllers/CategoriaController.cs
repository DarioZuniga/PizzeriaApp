using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.MVC.Models;
using PizzeriaApp.MVC.Security;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class CategoriaController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de la abstracci�n
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

        TempData["Success"] = "Categor�a creada correctamente.";

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

        TempData["Success"] = "Categor�a actualizada correctamente.";

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
                "Categor�a eliminada correctamente.";
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