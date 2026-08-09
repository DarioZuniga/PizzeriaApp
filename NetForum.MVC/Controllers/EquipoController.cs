using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.MVC.Models;
using NetForum.MVC.Security;

namespace NetForum.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class EquipoController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de las abstracción
    private readonly IEquipoService _equipoService;
    private readonly ILigaService _ligaService;

    public EquipoController(IEquipoService equipoService, ILigaService ligaService)
    {
        _equipoService = equipoService;
        _ligaService = ligaService;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var equipos = string.IsNullOrWhiteSpace(buscar)
            ? await _equipoService.ObtenerEquiposAsync()
            : await _equipoService.BuscarEquiposAsync(buscar);

        ViewBag.Buscar = buscar;
        ViewBag.Ligas = await ObtenerLigasSelectListAsync();

        return View(equipos);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var equipo = await _equipoService.ObtenerPorIdAsync(id);

        if (equipo is null)
        {
            return NotFound();
        }

        return Json(new
        {
            equipoId = equipo.EquipoId,
            ligaId = equipo.LigaId,
            nombre = equipo.Nombre,
            pais = equipo.Pais,
            fundacion = equipo.Fundacion
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAjax(EquipoViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inválidos.");
        }

        var equipo = new Equipo
        {
            LigaId = vm.LigaId,
            Nombre = vm.Nombre,
            Pais = vm.Pais,
            Fundacion = vm.Fundacion
        };

        await _equipoService.GuardarAsync(equipo);

        return Json(new
        {
            success = true,
            message = "Equipo creado correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAjax(int id, EquipoViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inválidos.");
        }

        var equipo = await _equipoService.ObtenerPorIdAsync(id);

        if (equipo is null)
        {
            return NotFound();
        }

        equipo.LigaId = vm.LigaId;
        equipo.Nombre = vm.Nombre;
        equipo.Pais = vm.Pais;
        equipo.Fundacion = vm.Fundacion;

        await _equipoService.ActualizarAsync(equipo);

        return Json(new
        {
            success = true,
            message = "Equipo actualizado correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAjax(int id)
    {
        await _equipoService.EliminarAsync(id);

        return Json(new
        {
            success = true,
            message = "Equipo eliminado correctamente."
        });
    }

    private async Task<List<SelectListItem>> ObtenerLigasSelectListAsync()
    {
        var ligas = await _ligaService.ObtenerLigasAsync();

        return ligas
            .Select(l => new SelectListItem
            {
                Value = l.LigaId.ToString(),
                Text = $"{l.Nombre} - {l.Pais}"
            })
            .ToList();
    }
}