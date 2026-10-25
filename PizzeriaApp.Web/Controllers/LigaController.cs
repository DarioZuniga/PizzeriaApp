using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.MVC.Models;
using PizzeriaApp.MVC.Security;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class LigaController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de la abstracci�n
    private readonly ILigaService _ligaService;

    public LigaController(ILigaService ligaService)
    {
        _ligaService = ligaService;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var ligas = string.IsNullOrWhiteSpace(buscar)
            ? await _ligaService.ObtenerLigasAsync()
            : await _ligaService.BuscarLigasAsync(buscar);

        ViewBag.Buscar = buscar;

        return View(ligas);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new LigaViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LigaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var liga = new Liga
        {
            Nombre = vm.Nombre,
            Pais = vm.Pais
        };

        await _ligaService.GuardarAsync(liga);

        TempData["Success"] = "Liga creada correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var liga = await _ligaService.ObtenerPorIdAsync(id);

        if (liga is null)
        {
            return NotFound();
        }

        var vm = new LigaViewModel
        {
            Nombre = liga.Nombre,
            Pais = liga.Pais
        };

        ViewBag.LigaId = liga.LigaId;

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, LigaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.LigaId = id;
            return View(vm);
        }

        var liga = await _ligaService.ObtenerPorIdAsync(id);

        if (liga is null)
        {
            return NotFound();
        }

        liga.Nombre = vm.Nombre;
        liga.Pais = vm.Pais;

        await _ligaService.ActualizarAsync(liga);

        TempData["Success"] = "Liga actualizada correctamente.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var liga = await _ligaService.ObtenerPorIdAsync(id);

        if (liga is null)
        {
            return NotFound();
        }

        return View(liga);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _ligaService.EliminarAsync(id);

        TempData["Success"] = "Liga eliminada correctamente.";

        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAjax(LigaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inv�lidos.");
        }

        var liga = new Liga
        {
            Nombre = vm.Nombre,
            Pais = vm.Pais
        };

        await _ligaService.GuardarAsync(liga);

        return Json(new
        {
            success = true,
            message = "Liga creada correctamente."
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var liga = await _ligaService.ObtenerPorIdAsync(id);

        if (liga is null)
        {
            return NotFound();
        }

        return Json(new
        {
            ligaId = liga.LigaId,
            nombre = liga.Nombre,
            pais = liga.Pais
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAjax(int id, LigaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inv�lidos.");
        }

        var liga = await _ligaService.ObtenerPorIdAsync(id);

        if (liga is null)
        {
            return NotFound();
        }

        liga.Nombre = vm.Nombre;
        liga.Pais = vm.Pais;

        await _ligaService.ActualizarAsync(liga);

        return Json(new
        {
            success = true,
            message = "Liga actualizada correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAjax(int id)
    {
        await _ligaService.EliminarAsync(id);

        return Json(new
        {
            success = true,
            message = "Liga eliminada correctamente."
        });
    }
}