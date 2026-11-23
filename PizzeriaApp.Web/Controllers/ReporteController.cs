using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using System.Text;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class ReporteController : Controller
{
    private readonly PizzeriaAppDbContext _context;
    public ReporteController(PizzeriaAppDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var hoy = DateTime.Today;
        var hace7Dias = hoy.AddDays(-7);
        var hace30Dias = hoy.AddDays(-30);

        // Pedidos por estado
        var pedidos = await _context.Pedidos.ToListAsync();
        ViewBag.TotalPedidos = pedidos.Count;
        ViewBag.Pendientes = pedidos.Count(p => p.Estado == "Pendiente");
        ViewBag.Pagados = pedidos.Count(p => p.Estado == "Pagado");
        ViewBag.EnPreparacion = pedidos.Count(p => p.Estado == "EnPreparacion");
        ViewBag.EnCamino = pedidos.Count(p => p.Estado == "EnCamino");
        ViewBag.Entregados = pedidos.Count(p => p.Estado == "Entregado");
        ViewBag.Cancelados = pedidos.Count(p => p.Estado == "Cancelado");

        // Ventas por período
        ViewBag.VentasHoy = pedidos.Where(p => p.Fecha.Date == hoy && p.Estado != "Cancelado").Sum(p => p.Total);
        ViewBag.VentasSemana = pedidos.Where(p => p.Fecha >= hace7Dias && p.Estado != "Cancelado").Sum(p => p.Total);
        ViewBag.VentasMes = pedidos.Where(p => p.Fecha >= hace30Dias && p.Estado != "Cancelado").Sum(p => p.Total);
        ViewBag.VentasTotal = pedidos.Where(p => p.Estado != "Cancelado").Sum(p => p.Total);

        // Ventas por día (últimos 7 días)
        var ventasPorDia = new List<object>();
        for (int i = 6; i >= 0; i--)
        {
            var dia = hoy.AddDays(-i);
            var total = pedidos.Where(p => p.Fecha.Date == dia && p.Estado != "Cancelado").Sum(p => p.Total);
            var cant = pedidos.Count(p => p.Fecha.Date == dia && p.Estado != "Cancelado");
            ventasPorDia.Add(new { Fecha = dia.ToString("dd/MM"), Total = total, Cantidad = cant });
        }
        ViewBag.VentasPorDia = ventasPorDia;

        // Productos más vendidos
        var productosMasVendidos = await _context.DetallesPedido
            .Include(d => d.Producto)
            .Include(d => d.Pedido)
            .Where(d => d.Pedido.Estado != "Cancelado")
            .GroupBy(d => new { d.ProductoId, d.Producto!.Nombre })
            .Select(g => new
            {
                Nombre = g.Key.Nombre,
                Cantidad = g.Sum(d => d.Cantidad),
                Total = g.Sum(d => d.PrecioUnitario * d.Cantidad)
            })
            .OrderByDescending(x => x.Cantidad)
            .Take(10)
            .ToListAsync();
        ViewBag.ProductosMasVendidos = productosMasVendidos;

        // Cupones más usados
        var cupones = await _context.Cupones.ToListAsync();
        ViewBag.TotalCupones = cupones.Count;
        ViewBag.CuponesActivos = cupones.Count(c => c.Activo);
        var cuponesMasUsados = cupones
            .OrderByDescending(c => c.UsosActuales)
            .Take(5)
            .Select(c => new { c.Codigo, c.UsosActuales, c.UsosMaximos, c.PorcentajeDescuento })
            .ToList();
        ViewBag.CuponesMasUsados = cuponesMasUsados;

        // Clientes frecuentes
        var clientesFrecuentes = await _context.Pedidos
            .Include(p => p.Usuario)
            .Where(p => p.Estado != "Cancelado")
            .GroupBy(p => new { p.UsuarioId, p.Usuario!.NombreUsuario })
            .Select(g => new
            {
                Nombre = g.Key.NombreUsuario,
                Pedidos = g.Count(),
                Total = g.Sum(p => p.Total)
            })
            .OrderByDescending(x => x.Total)
            .Take(5)
            .ToListAsync();
        ViewBag.ClientesFrecuentes = clientesFrecuentes;

        return View();
    }

    public async Task<IActionResult> ExportarCsv()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.Usuario)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("PedidoId,Fecha,Cliente,Estado,Subtotal,Impuestos,Total,MetodoPago");
        foreach (var p in pedidos)
        {
            sb.AppendLine($"{p.PedidoId},{p.Fecha:yyyy-MM-dd HH:mm},\"{p.Usuario?.NombreUsuario}\",{p.Estado},{p.Subtotal:F2},{p.Impuestos:F2},{p.Total:F2},{p.MetodoPago}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", $"ReportePedidos_{DateTime.Now:yyyyMMdd}.csv");
    }

    public async Task<IActionResult> ExportarProductosCsv()
    {
        var productos = await _context.DetallesPedido
            .Include(d => d.Producto)
            .Include(d => d.Pedido)
            .Where(d => d.Pedido.Estado != "Cancelado")
            .GroupBy(d => new { d.ProductoId, d.Producto!.Nombre })
            .Select(g => new
            {
                Nombre = g.Key.Nombre,
                Cantidad = g.Sum(d => d.Cantidad),
                Total = g.Sum(d => d.PrecioUnitario * d.Cantidad)
            })
            .OrderByDescending(x => x.Cantidad)
            .ToListAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Producto,CantidadVendida,TotalVendido");
        foreach (var p in productos)
        {
            sb.AppendLine($"\"{p.Nombre}\",{p.Cantidad},{p.Total:F2}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", $"ReporteProductos_{DateTime.Now:yyyyMMdd}.csv");
    }
}