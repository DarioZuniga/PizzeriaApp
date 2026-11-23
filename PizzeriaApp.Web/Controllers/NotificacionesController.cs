using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using System.Security.Claims;

namespace PizzeriaApp.MVC.Controllers;

[Authorize]
public class NotificacionesController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public NotificacionesController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    private int GetUsuarioId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    public async Task<IActionResult> Index()
    {
        var usuarioId = GetUsuarioId();
        var notifs = await _context.Notificaciones
            .Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.FechaCreacion)
            .ToListAsync();
        return View(notifs);
    }

    [HttpGet]
    public async Task<IActionResult> ContarNoLeidas()
    {
        var usuarioId = GetUsuarioId();
        var count = await _context.Notificaciones
            .CountAsync(n => n.UsuarioId == usuarioId && !n.Leida);
        return Json(new { count });
    }

    [HttpGet]
    public async Task<IActionResult> Ultimas()
    {
        var usuarioId = GetUsuarioId();
        var lista = await _context.Notificaciones
            .Where(n => n.UsuarioId == usuarioId)
            .OrderByDescending(n => n.FechaCreacion)
            .Take(5)
            .Select(n => new {
                        n.NotificacionId,
                        n.Mensaje,
                        n.Leida,
                        fecha = n.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
            })
            .ToListAsync();
        return Json(lista);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcarLeida(int id)
    {
        var usuarioId = GetUsuarioId();
        var notif = await _context.Notificaciones
            .FirstOrDefaultAsync(n => n.NotificacionId == id && n.UsuarioId == usuarioId);
        if (notif != null)
        {
            notif.Leida = true;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcarTodasLeidas()
    {
        var usuarioId = GetUsuarioId();
        var noLeidas = await _context.Notificaciones
            .Where(n => n.UsuarioId == usuarioId && !n.Leida)
            .ToListAsync();
        foreach (var n in noLeidas) n.Leida = true;
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = "Todas las notificaciones fueron marcadas como leídas.";
        return RedirectToAction(nameof(Index));
    }
}