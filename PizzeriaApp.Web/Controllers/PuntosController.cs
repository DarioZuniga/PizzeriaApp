using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using System.Security.Claims;

namespace PizzeriaApp.MVC.Controllers;

[Authorize]
public class PuntosController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public PuntosController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var historial = await _context.PuntosRecompensa
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        ViewBag.Saldo = historial.Where(p => p.Puntos > 0).Sum(p => p.Puntos) 
                      - Math.Abs(historial.Where(p => p.Puntos < 0).Sum(p => p.Puntos));

        return View(historial);
    }
}