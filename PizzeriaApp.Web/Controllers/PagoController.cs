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
    public async Task<IActionResult> Procesar(int pedidoId, string metodoPago, string? numeroTarjeta, string? fechaExpira, string? cvv, string? titular, string? comprobanteSinpe)
    {
        var pedido = await _context.Pedidos.FindAsync(pedidoId);
        if (pedido == null) return NotFound();

        // Validaciones simuladas
        if (metodoPago == "Tarjeta")
        {
            if (string.IsNullOrEmpty(numeroTarjeta) || numeroTarjeta.Length < 16)
            {
                TempData["Error"] = "El número de tarjeta debe tener 16 dígitos.";
                return RedirectToAction("Index", new { pedidoId });
            }
            if (string.IsNullOrEmpty(fechaExpira) || !fechaExpira.Contains("/"))
            {
                TempData["Error"] = "La fecha de expiración es inválida (MM/AA).";
                return RedirectToAction("Index", new { pedidoId });
            }
            if (string.IsNullOrEmpty(cvv) || cvv.Length < 3)
            {
                TempData["Error"] = "El CVV debe tener 3 dígitos.";
                return RedirectToAction("Index", new { pedidoId });
            }
            if (string.IsNullOrEmpty(titular))
            {
                TempData["Error"] = "El nombre del titular es obligatorio.";
                return RedirectToAction("Index", new { pedidoId });
            }
        }
        else if (metodoPago == "SINPE")
        {
            if (string.IsNullOrEmpty(comprobanteSinpe))
            {
                TempData["Error"] = "Debe ingresar el número de comprobante de SINPE.";
                return RedirectToAction("Index", new { pedidoId });
            }
        }

        // Simular aprobación
        pedido.MetodoPago = metodoPago;
        pedido.Estado = "Pagado";
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Pago procesado exitosamente.";
        return RedirectToAction("Seguimiento", "Pedido", new { id = pedidoId });
    }
}