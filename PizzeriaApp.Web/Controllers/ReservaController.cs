using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using System.Security.Claims;

namespace PizzeriaApp.MVC.Controllers;

[Authorize]
public class ReservaController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public ReservaController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(DateTime fechaHora, int cantidadPersonas, string notas)
    {
        // Validaciones
        if (fechaHora < DateTime.Now)
        {
            ModelState.AddModelError("", "La fecha y hora deben ser futuras.");
            return View();
        }

        var config = await _context.ConfiguracionesReserva.FirstOrDefaultAsync();
        if (config != null)
        {
            var hora = fechaHora.TimeOfDay;
            if (hora < config.HoraApertura || hora > config.HoraCierre)
            {
                ModelState.AddModelError("", $"Solo se aceptan reservas entre {config.HoraApertura:hh\\:mm} y {config.HoraCierre:hh\\:mm}.");
                return View();
            }

            // Verificar límite diario
            var reservasDelDia = await _context.Reservas
                .CountAsync(r => r.FechaHora.Date == fechaHora.Date && r.Estado != "Cancelada");
            
            if (reservasDelDia >= config.LimiteReservasPorDia)
            {
                ModelState.AddModelError("", "No hay disponibilidad para ese día.");
                return View();
            }
        }

        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var reserva = new Reserva
        {
            UsuarioId = usuarioId,
            FechaHora = fechaHora,
            CantidadPersonas = cantidadPersonas,
            Notas = notas,
            Estado = "Pendiente"
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Reserva creada exitosamente.";
        return RedirectToAction(nameof(MisReservas));
    }

    public async Task<IActionResult> MisReservas()
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var reservas = await _context.Reservas
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.FechaHora)
            .ToListAsync();
        return View(reservas);
    }

    [HttpPost]
    public async Task<IActionResult> Cancelar(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva != null)
        {
            reserva.Estado = "Cancelada";
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(MisReservas));
    }
    
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Gestion()
    {
        var reservas = await _context.Reservas
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.FechaHora)
            .ToListAsync();
        return View(reservas);
    }
    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null) return NotFound();
        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Reserva reserva)
    {
        if (ModelState.IsValid)
        {
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Reserva actualizada exitosamente.";
            return RedirectToAction(nameof(MisReservas));
        }
        return View(reserva);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Configuracion()
    {
        var config = await _context.ConfiguracionesReserva.FirstOrDefaultAsync();
        if (config == null)
        {
            config = new ConfiguracionReserva();
            _context.ConfiguracionesReserva.Add(config);
            await _context.SaveChangesAsync();
        }
        return View(config);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Configuracion(ConfiguracionReserva config)
    {
        if (ModelState.IsValid)
        {
            _context.ConfiguracionesReserva.Update(config);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Configuración guardada.";
            return RedirectToAction(nameof(Configuracion));
        }
        return View(config);
    }
}