using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.MVC.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.MVC.Controllers;

public class CarritoController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public CarritoController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    private List<CarritoItem> ObtenerCarrito()
    {
        var carritoJson = HttpContext.Session.GetString("Carrito");
        if (string.IsNullOrEmpty(carritoJson))
            return new List<CarritoItem>();
        return JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson) ?? new List<CarritoItem>();
    }

    private void GuardarCarrito(List<CarritoItem> carrito)
    {
        HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
    }

    public IActionResult Index()
    {
        var carrito = ObtenerCarrito();
        return View(carrito);
    }

        [HttpPost]
    public async Task<IActionResult> Agregar(int productoId, int cantidad, int? tamanoId, int? tipoMasaId, List<int>? ingredientesIds, decimal? precioFinal = null)
    {
        var producto = await _context.Productos.FindAsync(productoId);
        if (producto == null) return NotFound();

        var carrito = ObtenerCarrito();

        var item = new CarritoItem
        {
            ProductoId = productoId,
            Nombre = producto.Nombre,
            PrecioBase = producto.Precio,
            Cantidad = cantidad,
            TamanoId = tamanoId,
            TipoMasaId = tipoMasaId,
            IngredientesIds = ingredientesIds ?? new List<int>()
        };

        // Calcular precio del tamaño
        if (tamanoId.HasValue)
        {
            var tamano = await _context.Tamanos.FindAsync(tamanoId.Value);
            if (tamano != null)
            {
                item.TamanoNombre = tamano.Nombre;
                // El factor de precio se suma como un extra (ej. Grande = +30% del precio base)
                item.PrecioTamano = producto.Precio * (tamano.FactorPrecio - 1);
            }
        }

        // Calcular precio del tipo de masa
        if (tipoMasaId.HasValue)
        {
            var tipoMasa = await _context.TiposMasa.FindAsync(tipoMasaId.Value);
            if (tipoMasa != null)
            {
                item.TipoMasaNombre = tipoMasa.Nombre;
                item.PrecioMasa = tipoMasa.PrecioExtra;
            }
        }

        // Calcular precio de ingredientes
        if (ingredientesIds != null && ingredientesIds.Any())
        {
            var ingredientes = await _context.Ingredientes
                .Where(i => ingredientesIds.Contains(i.IngredienteId))
                .ToListAsync();
            item.IngredientesNombres = ingredientes.Select(i => i.Nombre).ToList();
            item.PrecioIngredientes = ingredientes.Sum(i => i.PrecioExtra);
        }

        carrito.Add(item);
        GuardarCarrito(carrito);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Eliminar(int index)
    {
        var carrito = ObtenerCarrito();
        if (index >= 0 && index < carrito.Count)
        {
            carrito.RemoveAt(index);
            GuardarCarrito(carrito);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmarPedido()
    {
        var carrito = ObtenerCarrito();
        if (!carrito.Any()) return RedirectToAction("Index");

        // Calcular totales
        decimal subtotal = carrito.Sum(i => i.PrecioTotal);
        decimal impuestos = subtotal * 0.13m;
        decimal total = subtotal + impuestos;

        // Crear pedido
        var pedido = new Pedido
        {
            UsuarioId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0"),
            Subtotal = subtotal,
            Impuestos = impuestos,
            Total = total,
            Estado = "Pendiente",
            MetodoPago = "PorDefinir"
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Crear detalles
        foreach (var item in carrito)
        {
            var detalle = new DetallePedido
            {
                PedidoId = pedido.PedidoId,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                PrecioUnitario = item.PrecioBase,
                TamanoId = item.TamanoId,
                TipoMasaId = item.TipoMasaId
            };

            _context.DetallesPedido.Add(detalle);
            await _context.SaveChangesAsync();

            // Crear ingredientes del detalle
            foreach (var ingId in item.IngredientesIds)
            {
                _context.DetallePedidoIngredientes.Add(new DetallePedidoIngrediente
                {
                    DetallePedidoId = detalle.DetallePedidoId,
                    IngredienteId = ingId,
                    PrecioExtra = 500m
                });
            }
        }

        await _context.SaveChangesAsync();

        // Limpiar carrito
        HttpContext.Session.Remove("Carrito");

        return RedirectToAction("Index", "Pago", new { pedidoId = pedido.PedidoId });
    }
    
    // Acción temporal para el Pago (HU_09)
    public IActionResult Pago(int pedidoId)
    {
        return Content($"Redirigiendo a pago del pedido {pedidoId}. (En el siguiente paso se implementa la pasarela simulada).");
    }
}