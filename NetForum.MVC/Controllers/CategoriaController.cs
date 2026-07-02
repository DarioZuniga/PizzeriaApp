using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.MVC.Models;

namespace NetForum.MVC.Controllers;

[Authorize]
public class CategoriaController : Controller
{
    private readonly ICategoriaService _categoriaService;

    public CategoriaController(ICategoriaService categoriaService)
    {
        // SOLID: Dependency Inversion Principle
        _categoriaService = categoriaService;
    }

    public async Task<IActionResult> Index()
    {
        var categorias = await _categoriaService.ObtenerCategoriasAsync();
        return View(categorias);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CategoriaViewModel
        {
            Nombre = string.Empty,
            Descripcion = string.Empty
        });
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

        return RedirectToAction(nameof(Index));
    }
}