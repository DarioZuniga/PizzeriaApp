using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class CuponController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public CuponController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var cupones = await _context.Cupones.ToListAsync();
        return View(cupones);
    }

    [HttpGet]
    public IActionResult Crear() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(Cupon cupon)
    {
        if (ModelState.IsValid)
        {
            _context.Cupones.Add(cupon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cupon);
    }

    [HttpPost]
    public async Task<IActionResult> Desactivar(int id)
    {
        var cupon = await _context.Cupones.FindAsync(id);
        if (cupon != null)
        {
            cupon.Activo = false;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}