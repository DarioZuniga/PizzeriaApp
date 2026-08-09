using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.MVC.Models;
using System.Security.Claims;
using NetForum.MVC.Security;

namespace NetForum.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class RespuestaController : Controller
{
    private readonly IRespuestaService _respuestaService;
    private readonly ITemaService _temaService;

    public RespuestaController(
        IRespuestaService respuestaService,
        ITemaService temaService)
    {
        // SOLID: Dependency Inversion Principle
        _respuestaService = respuestaService;
        _temaService = temaService;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var respuestas = string.IsNullOrWhiteSpace(buscar)
            ? await _respuestaService.ObtenerRespuestasAsync()
            : await _respuestaService.BuscarRespuestasAsync(buscar);

        ViewBag.Buscar = buscar;
        ViewBag.Temas = await ObtenerTemasSelectListAsync();

        return View(respuestas);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var respuesta = await _respuestaService.ObtenerPorIdAsync(id);

        if (respuesta is null)
        {
            return NotFound();
        }

        return Json(new
        {
            respuestaId = respuesta.RespuestaId,
            temaId = respuesta.TemaId,
            contenido = respuesta.Contenido
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAjax(RespuestaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inválidos.");
        }

        var usuarioId = ObtenerUsuarioId();

        if (usuarioId == 0)
        {
            return Unauthorized();
        }

        var respuesta = new Respuesta
        {
            TemaId = vm.TemaId,
            UsuarioId = usuarioId,
            EstadoId = 1,
            Contenido = vm.Contenido,
            FechaCreacion = DateTime.Now
        };

        await _respuestaService.GuardarAsync(respuesta);

        return Json(new
        {
            success = true,
            message = "Respuesta creada correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAjax(int id, RespuestaViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Datos inválidos.");
        }

        var respuesta = await _respuestaService.ObtenerPorIdAsync(id);

        if (respuesta is null)
        {
            return NotFound();
        }

        respuesta.TemaId = vm.TemaId;
        respuesta.Contenido = vm.Contenido;

        await _respuestaService.ActualizarAsync(respuesta);

        return Json(new
        {
            success = true,
            message = "Respuesta actualizada correctamente."
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAjax(int id)
    {
        await _respuestaService.EliminarAsync(id);

        return Json(new
        {
            success = true,
            message = "Respuesta eliminada correctamente."
        });
    }

    private int ObtenerUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(claim, out var usuarioId)
            ? usuarioId
            : 0;
    }

    private async Task<List<SelectListItem>> ObtenerTemasSelectListAsync()
    {
        var temas = await _temaService.ObtenerTemasAsync();

        return temas
            .Select(t => new SelectListItem
            {
                Value = t.TemaId.ToString(),
                Text = t.Titulo
            })
            .ToList();
    }
}