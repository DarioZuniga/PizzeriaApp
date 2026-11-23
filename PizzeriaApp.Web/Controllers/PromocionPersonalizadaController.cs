using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class PromocionPersonalizadaController : Controller
{
    private readonly PizzeriaAppDbContext _context;
    public PromocionPersonalizadaController(PizzeriaAppDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var promos = await _context.PromocionesPersonalizadas
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();

        ViewBag.TotalPromos = promos.Count;
        ViewBag.PromosActivas = promos.Count(p => p.Activa);
        ViewBag.TotalAplicadas = promos.Sum(p => p.VecesAplicada);
        ViewBag.TotalDescontado = promos.Sum(p => p.MontoDescontado);

        return View(promos);
    }

    [HttpGet]
    public IActionResult Crear() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(PromocionPersonalizada promo)
    {
        if (promo.MinimoCompras < 1)
            ModelState.AddModelError("MinimoCompras", "El mínimo de compras debe ser al menos 1.");
        if (promo.DiasVentana < 1)
            ModelState.AddModelError("DiasVentana", "La ventana de días debe ser al menos 1.");
        if (promo.PorcentajeDescuento <= 0 || promo.PorcentajeDescuento > 100)
            ModelState.AddModelError("PorcentajeDescuento", "El descuento debe estar entre 1 y 100.");

        if (ModelState.IsValid)
        {
            promo.Activa = true;
            promo.FechaCreacion = DateTime.Now;
            _context.PromocionesPersonalizadas.Add(promo);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = $"Promoción '{promo.Nombre}' creada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(promo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var promo = await _context.PromocionesPersonalizadas.FindAsync(id);
        if (promo == null) return NotFound();
        return View(promo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(PromocionPersonalizada promo)
    {
        if (promo.MinimoCompras < 1 || promo.DiasVentana < 1 || promo.PorcentajeDescuento <= 0 || promo.PorcentajeDescuento > 100)
        {
            ModelState.AddModelError("", "Verificá los valores ingresados.");
            return View(promo);
        }

        if (ModelState.IsValid)
        {
            _context.PromocionesPersonalizadas.Update(promo);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = $"Promoción '{promo.Nombre}' actualizada.";
            return RedirectToAction(nameof(Index));
        }
        return View(promo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActiva(int id)
    {
        var promo = await _context.PromocionesPersonalizadas.FindAsync(id);
        if (promo != null)
        {
            promo.Activa = !promo.Activa;
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = $"Promoción '{promo.Nombre}' {(promo.Activa ? "activada" : "desactivada")}.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var promo = await _context.PromocionesPersonalizadas.FindAsync(id);
        if (promo != null)
        {
            _context.PromocionesPersonalizadas.Remove(promo);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Promoción eliminada.";
        }
        return RedirectToAction(nameof(Index));
    }

    // Reporte de uso
    public async Task<IActionResult> Reporte()
    {
        var promos = await _context.PromocionesPersonalizadas
            .OrderByDescending(p => p.VecesAplicada)
            .ToListAsync();
        return View(promos);
    }
}