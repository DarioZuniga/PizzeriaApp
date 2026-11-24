using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.MVC.Models;
using System.Security.Claims;
using System.Text.Json;

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

        return JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson)
            ?? new List<CarritoItem>();
    }

    private void GuardarCarrito(List<CarritoItem> carrito)
    {
        HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
    }

    private async Task<Cupon?> ObtenerCuponValidoAsync(string? codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return null;

        codigo = codigo.Trim().ToUpperInvariant();

        var cupon = await _context.Cupones
            .FirstOrDefaultAsync(c => c.Codigo.ToUpper() == codigo);

        if (cupon == null ||
            !cupon.Activo ||
            cupon.FechaInicio > DateTime.Now ||
            cupon.FechaFin < DateTime.Now ||
            cupon.UsosActuales >= cupon.UsosMaximos ||
            cupon.PorcentajeDescuento <= 0 ||
            cupon.PorcentajeDescuento > 100)
        {
            return null;
        }

        return cupon;
    }

    public async Task<IActionResult> Index()
    {
        var carrito = ObtenerCarrito();
        var codigo = HttpContext.Session.GetString("CuponAplicadoCodigo");
        var cupon = await ObtenerCuponValidoAsync(codigo);

        if (!string.IsNullOrWhiteSpace(codigo) && cupon == null)
        {
            HttpContext.Session.Remove("CuponAplicadoCodigo");
            TempData["ErrorCupon"] = "El cupón ya no está disponible o ha vencido.";
        }

        decimal subtotal = carrito.Sum(i => i.PrecioTotal);
        decimal descuento = cupon == null
            ? 0
            : Math.Round(subtotal * cupon.PorcentajeDescuento / 100m, 2);

        decimal subtotalConDescuento = subtotal - descuento;
        decimal impuestos = Math.Round(subtotalConDescuento * 0.13m, 2);
        decimal total = subtotalConDescuento + impuestos;

        ViewBag.CodigoCupon = cupon?.Codigo;
        ViewBag.PorcentajeCupon = cupon?.PorcentajeDescuento ?? 0;
        ViewBag.Descuento = descuento;
        ViewBag.Subtotal = subtotal;
        ViewBag.Impuestos = impuestos;
        ViewBag.Total = total;

        return View(carrito);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AplicarCupon(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
        {
            TempData["ErrorCupon"] = "Ingresá un código de cupón.";
            return RedirectToAction(nameof(Index));
        }

        codigo = codigo.Trim().ToUpperInvariant();

        var cupon = await ObtenerCuponValidoAsync(codigo);

        if (cupon == null)
        {
            HttpContext.Session.Remove("CuponAplicadoCodigo");
            TempData["ErrorCupon"] = "El cupón no existe, está inactivo, venció o agotó sus usos.";
            return RedirectToAction(nameof(Index));
        }

        HttpContext.Session.SetString("CuponAplicadoCodigo", cupon.Codigo);
        TempData["ExitoCupon"] = $"Cupón {cupon.Codigo} aplicado: {cupon.PorcentajeDescuento}% de descuento.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult QuitarCupon()
    {
        HttpContext.Session.Remove("CuponAplicadoCodigo");
        TempData["ExitoCupon"] = "Se quitó el cupón del carrito.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Agregar(
        int productoId,
        int cantidad,
        int? tamanoId,
        int? tipoMasaId,
        List<int>? ingredientesIds,
        string? notas = null)
    {
        var producto = await _context.Productos.FindAsync(productoId);
        if (producto == null)
            return NotFound();

        var carrito = ObtenerCarrito();

        var item = new CarritoItem
        {
            ProductoId = productoId,
            Nombre = producto.Nombre,
            PrecioBase = producto.Precio,
            Cantidad = cantidad < 1 ? 1 : cantidad,
            TamanoId = tamanoId,
            TipoMasaId = tipoMasaId,
            IngredientesIds = ingredientesIds ?? new List<int>(),
            Notas = notas
        };

        if (tamanoId.HasValue)
        {
            var tamano = await _context.Tamanos.FindAsync(tamanoId.Value);
            if (tamano != null)
            {
                item.TamanoNombre = tamano.Nombre;
                item.PrecioTamano = producto.Precio * (tamano.FactorPrecio - 1);
            }
        }

        if (tipoMasaId.HasValue)
        {
            var tipoMasa = await _context.TiposMasa.FindAsync(tipoMasaId.Value);
            if (tipoMasa != null)
            {
                item.TipoMasaNombre = tipoMasa.Nombre;
                item.PrecioMasa = tipoMasa.PrecioExtra;
            }
        }

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

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult ActualizarCantidad(int index, int cantidad)
    {
        var carrito = ObtenerCarrito();

        if (index >= 0 && index < carrito.Count)
        {
            carrito[index].Cantidad = Math.Clamp(cantidad, 1, 99);
            GuardarCarrito(carrito);
        }

        return RedirectToAction(nameof(Index));
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

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult EliminarIngrediente(int itemIndex, int ingredienteId)
    {
        var carrito = ObtenerCarrito();

        if (itemIndex >= 0 && itemIndex < carrito.Count)
        {
            var item = carrito[itemIndex];
            var ingrediente = _context.Ingredientes.Find(ingredienteId);

            if (ingrediente != null && item.IngredientesIds.Contains(ingredienteId))
            {
                item.IngredientesIds.Remove(ingredienteId);
                item.IngredientesNombres.Remove(ingrediente.Nombre);
                item.PrecioIngredientes -= ingrediente.PrecioExtra;
                GuardarCarrito(carrito);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarPedido()
    {
        var carrito = ObtenerCarrito();

        if (!carrito.Any())
        {
            TempData["ErrorCupon"] = "Tu carrito está vacío.";
            return RedirectToAction(nameof(Index));
        }

        var usuarioId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        if (usuarioId <= 0)
        {
            TempData["ErrorCupon"] = "No se pudo identificar al usuario. Iniciá sesión nuevamente.";
            return RedirectToAction(nameof(Index));
        }

        var subtotal = carrito.Sum(i => i.PrecioTotal);
        var codigoCupon = HttpContext.Session.GetString("CuponAplicadoCodigo");
        var cuponAplicado = await ObtenerCuponValidoAsync(codigoCupon);

        if (!string.IsNullOrWhiteSpace(codigoCupon) && cuponAplicado == null)
        {
            HttpContext.Session.Remove("CuponAplicadoCodigo");
            TempData["ErrorCupon"] = "El cupón dejó de estar disponible. Revisá el carrito e intentá nuevamente.";
            return RedirectToAction(nameof(Index));
        }

        decimal descuento = 0;

        // El cupón manual tiene prioridad. No se acumula con promociones personalizadas.
        if (cuponAplicado != null)
        {
            descuento = subtotal * cuponAplicado.PorcentajeDescuento / 100m;
        }
        else
        {
            var promosActivas = await _context.PromocionesPersonalizadas
                .Where(p => p.Activa)
                .ToListAsync();

            foreach (var promo in promosActivas)
            {
                var fechaVentana = DateTime.Now.AddDays(-promo.DiasVentana);

                var comprasEnVentana = await _context.Pedidos.CountAsync(p =>
                    p.UsuarioId == usuarioId &&
                    p.Fecha >= fechaVentana &&
                    p.Estado != "Cancelado");

                if (comprasEnVentana >= promo.MinimoCompras)
                {
                    var porcentaje = Math.Clamp(promo.PorcentajeDescuento, 0m, 100m);
                    descuento = subtotal * porcentaje / 100m;
                    break;
                }
            }
        }

        descuento = Math.Round(Math.Clamp(descuento, 0m, subtotal), 2);
        var subtotalConDescuento = subtotal - descuento;
        var impuestos = Math.Round(subtotalConDescuento * 0.13m, 2);
        var total = subtotalConDescuento + impuestos;

        await using var transaccion = await _context.Database.BeginTransactionAsync();

        try
        {
            // Comprobar de nuevo el cupón dentro del proceso de confirmación.
            if (cuponAplicado != null)
            {
                cuponAplicado = await _context.Cupones
                    .FirstOrDefaultAsync(c => c.CuponId == cuponAplicado.CuponId);

                if (cuponAplicado == null ||
                    !cuponAplicado.Activo ||
                    cuponAplicado.FechaInicio > DateTime.Now ||
                    cuponAplicado.FechaFin < DateTime.Now ||
                    cuponAplicado.UsosActuales >= cuponAplicado.UsosMaximos ||
                    cuponAplicado.PorcentajeDescuento <= 0 ||
                    cuponAplicado.PorcentajeDescuento > 100)
                {
                    await transaccion.RollbackAsync();
                    HttpContext.Session.Remove("CuponAplicadoCodigo");
                    TempData["ErrorCupon"] = "El cupón ya no está disponible. No se creó el pedido.";
                    return RedirectToAction(nameof(Index));
                }

                cuponAplicado.UsosActuales++;
            }

            var pedido = new Pedido
            {
                UsuarioId = usuarioId,
                Subtotal = subtotal,
                Impuestos = impuestos,
                Total = total,
                CodigoCupon = cuponAplicado?.Codigo,
                Descuento = descuento,
                Estado = "Pendiente",
                MetodoPago = "PorDefinir"
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            foreach (var item in carrito)
            {
                var precioUnitarioReal =
                    item.PrecioBase +
                    item.PrecioTamano +
                    item.PrecioMasa +
                    item.PrecioIngredientes;

                if (precioUnitarioReal < 0)
                    precioUnitarioReal = 0;

                var detalle = new DetallePedido
                {
                    PedidoId = pedido.PedidoId,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = precioUnitarioReal,
                    TamanoId = item.TamanoId,
                    TipoMasaId = item.TipoMasaId
                };

                _context.DetallesPedido.Add(detalle);
                await _context.SaveChangesAsync();

                if (item.IngredientesIds.Any())
                {
                    var ingredientesDb = await _context.Ingredientes
                        .Where(i => item.IngredientesIds.Contains(i.IngredienteId))
                        .ToListAsync();

                    foreach (var ingrediente in ingredientesDb)
                    {
                        _context.DetallePedidoIngredientes.Add(
                            new DetallePedidoIngrediente
                            {
                                DetallePedidoId = detalle.DetallePedidoId,
                                IngredienteId = ingrediente.IngredienteId,
                                PrecioExtra = ingrediente.PrecioExtra
                            });
                    }
                }
            }

            await _context.SaveChangesAsync();
            await transaccion.CommitAsync();

            HttpContext.Session.Remove("Carrito");
            HttpContext.Session.Remove("CuponAplicadoCodigo");

            TempData["PedidoExito"] = "Tu pedido se creó correctamente.";
            return RedirectToAction("Index", "Pago", new { pedidoId = pedido.PedidoId });
        }
        catch
        {
            await transaccion.RollbackAsync();
            throw;
        }
    }

    public IActionResult Pago(int pedidoId)
    {
        return Content(
            $"Redirigiendo a pago del pedido {pedidoId}. (En el siguiente paso se implementa la pasarela simulada).");
    }
}
