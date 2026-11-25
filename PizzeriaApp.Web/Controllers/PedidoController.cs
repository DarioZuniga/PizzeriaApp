using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.MVC.Controllers;

public class PedidoController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public PedidoController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

        [Authorize]
    public async Task<IActionResult> Seguimiento(int id)
    {
        var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        var pedido = await _context.Pedidos
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Tamano)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.TipoMasa)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Ingredientes)
                    .ThenInclude(i => i.Ingrediente)
            .FirstOrDefaultAsync(p => p.PedidoId == id && p.UsuarioId == usuarioId);
        
        if (pedido == null) return NotFound();
        return View(pedido);
    }

    [Authorize]
    public async Task<IActionResult> Historial()
    {
        var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        var pedidos = await _context.Pedidos
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();
        
        // HU_16 C2: marcar pedidos ya evaluados
        var pedidosIds = pedidos.Select(p => p.PedidoId).ToList();
        var evaluados = await _context.Evaluaciones
            .Where(e => pedidosIds.Contains(e.PedidoId))
            .Select(e => e.PedidoId)
            .ToListAsync();
        ViewBag.Evaluados = evaluados;

        return View(pedidos);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Gestion()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.Usuario)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();
        return View(pedidos);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> CambiarEstado(int pedidoId, string nuevoEstado)
    {
        var pedido = await _context.Pedidos.FindAsync(pedidoId);

        if (pedido != null)
        {
            var estadoAnterior = pedido.Estado;

            if (!string.Equals(estadoAnterior, nuevoEstado, StringComparison.OrdinalIgnoreCase))
            {
                pedido.Estado = nuevoEstado;

                _context.Notificaciones.Add(new Notificacion
                {
                    UsuarioId = pedido.UsuarioId,
                    Mensaje = $"El estado de tu pedido #{pedido.PedidoId} cambió de {estadoAnterior} a {nuevoEstado}."
                });

                await _context.SaveChangesAsync();
            }
        }

        return RedirectToAction(nameof(Gestion));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Repetir(int id)
    {
        var usuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

        var pedido = await _context.Pedidos
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Tamano)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.TipoMasa)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Ingredientes)
                    .ThenInclude(i => i.Ingrediente)
            .FirstOrDefaultAsync(p => p.PedidoId == id && p.UsuarioId == usuarioId);

        if (pedido == null)
        {
            TempData["Error"] = "Pedido no encontrado.";
            return RedirectToAction(nameof(Historial));
        }

        // Cargar carrito actual desde sesiÃ³n
        var carritoJson = HttpContext.Session.GetString("Carrito");
        var carrito = string.IsNullOrEmpty(carritoJson)
            ? new List<PizzeriaApp.MVC.Models.CarritoItem>()
            : System.Text.Json.JsonSerializer.Deserialize<List<PizzeriaApp.MVC.Models.CarritoItem>>(carritoJson) ?? new List<PizzeriaApp.MVC.Models.CarritoItem>();

        // Copiar cada detalle del pedido al carrito
        foreach (var detalle in pedido.Detalles)
        {
            var item = new PizzeriaApp.MVC.Models.CarritoItem
            {
                ProductoId = detalle.ProductoId,
                Nombre = detalle.Producto?.Nombre ?? "Producto",
                PrecioBase = detalle.Producto?.Precio ?? detalle.PrecioUnitario,
                Cantidad = detalle.Cantidad,
                TamanoId = detalle.TamanoId,
                TamanoNombre = detalle.Tamano?.Nombre,
                TipoMasaId = detalle.TipoMasaId,
                TipoMasaNombre = detalle.TipoMasa?.Nombre,
                IngredientesIds = detalle.Ingredientes.Select(i => i.IngredienteId).ToList(),
                IngredientesNombres = detalle.Ingredientes.Select(i => i.Ingrediente?.Nombre ?? "").ToList()
            };

            // Recalcular precios de personalizaciÃ³n
            if (detalle.Tamano != null && detalle.Producto != null)
                item.PrecioTamano = detalle.Producto.Precio * (detalle.Tamano.FactorPrecio - 1);
            if (detalle.TipoMasa != null)
                item.PrecioMasa = detalle.TipoMasa.PrecioExtra;
            item.PrecioIngredientes = detalle.Ingredientes.Sum(i => i.PrecioExtra);

            carrito.Add(item);
        }

        HttpContext.Session.SetString("Carrito", System.Text.Json.JsonSerializer.Serialize(carrito));

        TempData["Mensaje"] = "Pedido copiado al carrito. RevisÃ¡ y confirmÃ¡.";
        return RedirectToAction("Index", "Carrito");
    }
}