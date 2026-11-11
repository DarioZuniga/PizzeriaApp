using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class InventarioController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public InventarioController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var insumos = await _context.Insumos.Where(i => i.Activo).ToListAsync();
        return View(insumos);
    }

    [HttpGet]
    public IActionResult CrearInsumo() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearInsumo(Insumo insumo)
    {
        if (ModelState.IsValid)
        {
            _context.Insumos.Add(insumo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(insumo);
    }

    [HttpGet]
    public async Task<IActionResult> Movimientos(int id)
    {
        var insumo = await _context.Insumos.FindAsync(id);
        if (insumo == null) return NotFound();

        ViewBag.InsumoId = id;
        ViewBag.InsumoNombre = insumo.Nombre;
        
        var movimientos = await _context.MovimientosInventario
            .Where(m => m.InsumoId == id)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();

        return View(movimientos);
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarMovimiento(int insumoId, string tipoMovimiento, decimal cantidad, string motivo)
    {
        var insumo = await _context.Insumos.FindAsync(insumoId);
        if (insumo == null) return NotFound();

        var movimiento = new MovimientoInventario
        {
            InsumoId = insumoId,
            TipoMovimiento = tipoMovimiento,
            Cantidad = cantidad,
            CostoUnitario = insumo.CostoUnitario,
            Motivo = motivo,
            UsuarioResponsable = User.Identity?.Name ?? "Admin"
        };

        // Actualizar stock según el tipo
        if (tipoMovimiento == "Entrada")
            insumo.StockActual += cantidad;
        else if (tipoMovimiento == "Salida" || tipoMovimiento == "Merma")
            insumo.StockActual -= cantidad;
        else if (tipoMovimiento == "Ajuste")
            insumo.StockActual = cantidad; // En ajuste, la cantidad es el nuevo stock real

        _context.MovimientosInventario.Add(movimiento);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Movimientos), new { id = insumoId });
    }
}