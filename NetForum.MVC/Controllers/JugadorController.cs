using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.MVC.Models;

namespace NetForum.MVC.Controllers;

[Authorize]
public class JugadorController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de las abstracciones
    private readonly IJugadorService _jugadorService;
    private readonly IEquipoService _equipoService;

    public JugadorController(IJugadorService jugadorService, IEquipoService equipoService)
    {
        _jugadorService = jugadorService;
        _equipoService = equipoService;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var jugadores = string.IsNullOrWhiteSpace(buscar)
            ? await _jugadorService.ObtenerJugadoresAsync()
            : await _jugadorService.BuscarJugadoresAsync(buscar);

        ViewBag.Buscar = buscar;
        ViewBag.Equipos = await ObtenerEquiposSelectListAsync();

        return View(jugadores);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var jugador = await _jugadorService.ObtenerPorIdAsync(id);

        if (jugador is null)
        {
            return NotFound();
        }

        return Json(new
        {
            jugadorId = jugador.JugadorId,
            equipoId = jugador.EquipoId,
            nombre = jugador.Nombre,
            posicion = jugador.Posicion,
            nacionalidad = jugador.Nacionalidad
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAjax(JugadorViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inválidos.");
        }

        var jugador = new Jugador
        {
            EquipoId = vm.EquipoId,
            Nombre = vm.Nombre,
            Posicion = vm.Posicion,
            Nacionalidad = vm.Nacionalidad
        };

        await _jugadorService.GuardarAsync(jugador);

        return Json(new
        {
            success = true,
            message = "Jugador creado correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAjax(int id, JugadorViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inválidos.");
        }

        var jugador = await _jugadorService.ObtenerPorIdAsync(id);

        if (jugador is null)
        {
            return NotFound();
        }

        jugador.EquipoId = vm.EquipoId;
        jugador.Nombre = vm.Nombre;
        jugador.Posicion = vm.Posicion;
        jugador.Nacionalidad = vm.Nacionalidad;

        await _jugadorService.ActualizarAsync(jugador);

        return Json(new
        {
            success = true,
            message = "Jugador actualizado correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAjax(int id)
    {
        await _jugadorService.EliminarAsync(id);

        return Json(new
        {
            success = true,
            message = "Jugador eliminado correctamente."
        });
    }

    private async Task<List<SelectListItem>> ObtenerEquiposSelectListAsync()
    {
        var equipos = await _equipoService.ObtenerEquiposAsync();

        return equipos
            .Select(e => new SelectListItem
            {
                Value = e.EquipoId.ToString(),
                Text = $"{e.Nombre} - {e.Pais}"
            })
            .ToList();
    }
}