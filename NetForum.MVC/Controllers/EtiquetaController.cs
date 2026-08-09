using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.MVC.Models;
using NetForum.MVC.Security;

namespace NetForum.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class EtiquetaController : Controller
{
    private readonly IEtiquetaService _etiquetaService;

    public EtiquetaController(IEtiquetaService etiquetaService)
    {
        // SOLID: Dependency Inversion Principle
        _etiquetaService = etiquetaService;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var etiquetas = string.IsNullOrWhiteSpace(buscar)
            ? await _etiquetaService.ObtenerEtiquetasAsync()
            : await _etiquetaService.BuscarEtiquetasAsync(buscar);

        ViewBag.Buscar = buscar;

        return View(etiquetas);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var etiqueta = await _etiquetaService.ObtenerPorIdAsync(id);

        if (etiqueta is null)
        {
            return NotFound();
        }

        return Json(new
        {
            etiquetaId = etiqueta.EtiquetaId,
            nombre = etiqueta.Nombre
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAjax(EtiquetaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inválidos.");
        }

        var etiqueta = new Etiqueta
        {
            Nombre = vm.Nombre
        };

        await _etiquetaService.GuardarAsync(etiqueta);

        return Json(new
        {
            success = true,
            message = "Etiqueta creada correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAjax(int id, EtiquetaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inválidos.");
        }

        var etiqueta = await _etiquetaService.ObtenerPorIdAsync(id);

        if (etiqueta is null)
        {
            return NotFound();
        }

        etiqueta.Nombre = vm.Nombre;

        await _etiquetaService.ActualizarAsync(etiqueta);

        return Json(new
        {
            success = true,
            message = "Etiqueta actualizada correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAjax(int id)
    {
        await _etiquetaService.EliminarAsync(id);

        return Json(new
        {
            success = true,
            message = "Etiqueta eliminada correctamente."
        });
    }
}