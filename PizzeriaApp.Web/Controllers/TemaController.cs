using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.MVC.Models;
using System.Security.Claims;
using PizzeriaApp.MVC.Security;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class TemaController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de las abstracciones
    private readonly ITemaService _temaService;
    private readonly ICategoriaService _categoriaService;

    public TemaController(
        ITemaService temaService,
        ICategoriaService categoriaService)
    {
        _temaService = temaService;
        _categoriaService = categoriaService;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var temas = string.IsNullOrWhiteSpace(buscar)
            ? await _temaService.ObtenerTemasAsync()
            : await _temaService.BuscarTemasAsync(buscar);

        ViewBag.Buscar = buscar;
        ViewBag.Categorias = await ObtenerCategoriasSelectListAsync();

        return View(temas);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var tema = await _temaService.ObtenerPorIdAsync(id);

        if (tema is null)
        {
            return NotFound();
        }

        return Json(new
        {
            temaId = tema.TemaId,
            categoriaId = tema.CategoriaId,
            titulo = tema.Titulo,
            contenido = tema.Contenido
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAjax(TemaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inv�lidos.");
        }

        var usuarioId = ObtenerUsuarioId();

        if (usuarioId == 0)
        {
            return Unauthorized();
        }

        var tema = new Tema
        {
            UsuarioId = usuarioId,
            CategoriaId = vm.CategoriaId,
            EstadoId = 1,
            Titulo = vm.Titulo,
            Contenido = vm.Contenido,
            FechaCreacion = DateTime.Now
        };

        await _temaService.GuardarAsync(tema);

        return Json(new
        {
            success = true,
            message = "Tema creado correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAjax(int id, TemaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inv�lidos.");
        }

        var tema = await _temaService.ObtenerPorIdAsync(id);

        if (tema is null)
        {
            return NotFound();
        }

        tema.CategoriaId = vm.CategoriaId;
        tema.Titulo = vm.Titulo;
        tema.Contenido = vm.Contenido;

        await _temaService.ActualizarAsync(tema);

        return Json(new
        {
            success = true,
            message = "Tema actualizado correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAjax(int id)
    {
        await _temaService.EliminarAsync(id);

        return Json(new
        {
            success = true,
            message = "Tema eliminado correctamente."
        });
    }

    private int ObtenerUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(claim, out var usuarioId)
            ? usuarioId
            : 0;
    }

    private async Task<List<SelectListItem>> ObtenerCategoriasSelectListAsync()
    {
        var categorias = await _categoriaService.ObtenerCategoriasAsync();

        return categorias
            .Select(c => new SelectListItem
            {
                Value = c.CategoriaId.ToString(),
                Text = c.Nombre
            })
            .ToList();
    }
}