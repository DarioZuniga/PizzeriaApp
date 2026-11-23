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
            pedido.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Gestion));
    }
}