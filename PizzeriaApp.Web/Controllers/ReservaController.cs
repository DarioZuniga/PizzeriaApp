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

            var reservasDelDia = await _context.Reservas
                .CountAsync(r => r.FechaHora.Date == fechaHora.Date && r.Estado != "Cancelada");
            
            if (reservasDelDia >= config.LimiteReservasPorDia)
            {
                ModelState.AddModelError("", "No hay disponibilidad para ese dÃ­a.");
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
            Estado = "Pendiente",
            CodigoConfirmacion = "RES-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper()
        };

        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Reserva creada exitosamente.";
        return RedirectToAction(nameof(MisReservas));
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerHorariosDisponibles(string fecha)
    {
        if (!DateTime.TryParse(fecha, out DateTime fechaSeleccionada))
            return Json(new { error = "Fecha invÃ¡lida" });

        var config = await _context.ConfiguracionesReserva.FirstOrDefaultAsync();
        if (config == null)
            return Json(new { error = "No hay configuraciÃ³n de horarios" });

        var reservasDelDia = await _context.Reservas
            .Where(r => r.FechaHora.Date == fechaSeleccionada.Date && r.Estado != "Cancelada")
            .Select(r => r.FechaHora.TimeOfDay)
            .ToListAsync();

        var horarios = new List<string>();
        var horaActual = config.HoraApertura;
        
        while (horaActual < config.HoraCierre)
        {
            if (!reservasDelDia.Contains(horaActual))
            {
                horarios.Add(horaActual.ToString(@"hh\:mm"));
            }
            horaActual = horaActual.Add(TimeSpan.FromMinutes(30));
        }

        return Json(horarios);
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
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var reserva = await _context.Reservas.FindAsync(id);

        if (reserva == null || reserva.UsuarioId != usuarioId)
        {
            TempData["Error"] = "Reserva no encontrada.";
            return RedirectToAction(nameof(MisReservas));
        }

        if (reserva.FechaHora < DateTime.Now)
        {
            TempData["Error"] = "No se puede cancelar una reserva pasada.";
            return RedirectToAction(nameof(MisReservas));
        }

        if (reserva.Estado == "Cancelada")
        {
            TempData["Error"] = "La reserva ya está cancelada.";
            return RedirectToAction(nameof(MisReservas));
        }

        reserva.Estado = "Cancelada";
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Reserva {reserva.CodigoConfirmacion} cancelada. Se notificó al cliente (simulado).";
        return RedirectToAction(nameof(MisReservas));
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
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        var original = await _context.Reservas.AsNoTracking().FirstOrDefaultAsync(r => r.ReservaId == reserva.ReservaId);
        if (original == null || original.UsuarioId != usuarioId)
        {
            TempData["Error"] = "Reserva no encontrada.";
            return RedirectToAction(nameof(MisReservas));
        }

        if (reserva.FechaHora < DateTime.Now)
        {
            ModelState.AddModelError("FechaHora", "La fecha y hora deben ser futuras.");
            return View(reserva);
        }

        var config = await _context.ConfiguracionesReserva.FirstOrDefaultAsync();
        if (config != null)
        {
            var hora = reserva.FechaHora.TimeOfDay;
            if (hora < config.HoraApertura || hora > config.HoraCierre)
            {
                ModelState.AddModelError("FechaHora", $"Solo se aceptan reservas entre {config.HoraApertura:hh\\:mm} y {config.HoraCierre:hh\\:mm}.");
                return View(reserva);
            }
        }

        // Mantener el estado y código originales (no permitir que el cliente los cambie)
        reserva.UsuarioId = usuarioId;
        reserva.Estado = original.Estado;
        reserva.CodigoConfirmacion = original.CodigoConfirmacion;

        _context.Reservas.Update(reserva);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"Reserva {reserva.CodigoConfirmacion} actualizada. Notificación enviada al cliente (simulada).";
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
            TempData["Mensaje"] = "ConfiguraciÃ³n guardada.";
            return RedirectToAction(nameof(Configuracion));
        }
        return View(config);
    }
}