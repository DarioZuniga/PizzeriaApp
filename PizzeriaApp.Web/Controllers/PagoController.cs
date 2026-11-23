using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.MVC.Controllers;

public class PagoController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    private readonly PizzeriaApp.MVC.Services.FacturaService _facturaService;
    public PagoController(PizzeriaAppDbContext context, PizzeriaApp.MVC.Services.FacturaService facturaService)
    {
        _context = context;
    
        _facturaService = facturaService;
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
                TempData["Error"] = "El nÃºmero de tarjeta debe tener 16 dÃ­gitos.";
                return RedirectToAction("Index", new { pedidoId });
            }
            if (string.IsNullOrEmpty(fechaExpira) || !fechaExpira.Contains("/"))
            {
                TempData["Error"] = "La fecha de expiraciÃ³n es invÃ¡lida (MM/AA).";
                return RedirectToAction("Index", new { pedidoId });
            }
            if (string.IsNullOrEmpty(cvv) || cvv.Length < 3)
            {
                TempData["Error"] = "El CVV debe tener 3 dÃ­gitos.";
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
                TempData["Error"] = "Debe ingresar el nÃºmero de comprobante de SINPE.";
                return RedirectToAction("Index", new { pedidoId });
            }
        }

        // Simular aprobaciÃ³n
        pedido.MetodoPago = metodoPago;
        pedido.Estado = "Pagado";

        // ===== HU_15: Acumular puntos de recompensa =====
        var puntosGanados = (int)Math.Floor(pedido.Total / 1000m);
        if (puntosGanados > 0)
        {
            _context.PuntosRecompensa.Add(new PuntoRecompensa
            {
                UsuarioId = pedido.UsuarioId,
                Puntos = puntosGanados,
                Fecha = DateTime.Now,
                Motivo = $"Compra pedido #{pedido.PedidoId:D4} - â‚¡{pedido.Total:N0}"
            });

            // ===== HU_15 C3: NotificaciÃ³n interna =====
            _context.Notificaciones.Add(new Notificacion
            {
                UsuarioId = pedido.UsuarioId,
                Mensaje = $"Â¡Ganaste {puntosGanados} puntos por tu compra! RevisÃ¡ tu saldo en 'Mis Puntos'.",
                Leida = false,
                FechaCreacion = DateTime.Now
            });

            TempData["Mensaje"] = $"Pago procesado exitosamente. Â¡Ganaste {puntosGanados} puntos!";
        }
        else
        {
            TempData["Mensaje"] = "Pago procesado exitosamente.";
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Exito", new { pedidoId });
    }

    public async Task<IActionResult> Exito(int pedidoId)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Ingredientes)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

        if (pedido == null) return NotFound();

        ViewBag.NumeroFactura = $"FAC-{DateTime.Now:yyyyMMdd}-{pedido.PedidoId:D4}";
        return View(pedido);
    }

    // HU_18: Descargar factura PDF
    [HttpGet]
    public async Task<IActionResult> DescargarFactura(int pedidoId)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

        if (pedido == null) return NotFound();

        var pdf = _facturaService.GenerarFacturaPdf(pedido);
        var nombreArchivo = $"Factura_{pedido.PedidoId:D4}_{pedido.Fecha:yyyyMMdd}.pdf";

        return File(pdf, "application/pdf", nombreArchivo);
    }
}