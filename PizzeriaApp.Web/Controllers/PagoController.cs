using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.MVC.Controllers;

public class PagoController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public PagoController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int pedidoId)
    {
        var pedido = await _context.Pedidos.FindAsync(pedidoId);
        if (pedido == null) return NotFound();
        return View(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> Procesar(int pedidoId, string metodoPago)
    {
        var pedido = await _context.Pedidos.FindAsync(pedidoId);
        if (pedido == null) return NotFound();

        pedido.MetodoPago = metodoPago;
        pedido.Estado = "Pagado";
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Pago procesado exitosamente.";
        return RedirectToAction("Seguimiento", "Pedido", new { id = pedidoId });
    }
}