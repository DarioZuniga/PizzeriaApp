using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using System.Security.Claims;

namespace PizzeriaApp.MVC.Controllers;

[Authorize]
public class DireccionController : Controller
{
    private readonly PizzeriaAppDbContext _context;
    public DireccionController(PizzeriaAppDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        return View(await _context.Direcciones.Where(d => d.UsuarioId == userId).ToListAsync());
    }

    [HttpGet]
    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(Direccion direccion)
    {
        if (ModelState.IsValid)
        {
            direccion.UsuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            _context.Direcciones.Add(direccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(direccion);
    }
    
    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        var dir = await _context.Direcciones.FindAsync(id);
        if (dir != null) { _context.Direcciones.Remove(dir); await _context.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}