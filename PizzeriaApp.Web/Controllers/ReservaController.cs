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
    public async Task<IActionResult> Crear(DateTime fechaHora, int cantidadPersonas, string? notas)
    {
        var error = await ValidarReservaAsync(fechaHora, cantidadPersonas);

        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View();
        }

        var usuarioId = ObtenerUsuarioId();

        if (usuarioId == 0)
            return Forbid();

        var reserva = new Reserva
        {
            UsuarioId = usuarioId,
            FechaHora = fechaHora,
            CantidadPersonas = cantidadPersonas,
            Notas = notas?.Trim() ?? string.Empty,
            Estado = "Pendiente",
            CodigoConfirmacion = "RES-" + Guid.NewGuid().ToString("N")[..6].ToUpper()
        };

        _context.Reservas.Add(reserva);

        // Notificar a todos los administradores
        var administradoresIds = await _context.Usuarios
            .Where(u => u.RolId == 1)
            .Select(u => u.UsuarioId)
            .ToListAsync();

        foreach (var administradorId in administradoresIds)
        {
            _context.Notificaciones.Add(new Notificacion
            {
                UsuarioId = administradorId,
                Mensaje = $"Nueva solicitud de reserva {reserva.CodigoConfirmacion} para {reserva.FechaHora:dd/MM/yyyy HH:mm} ({reserva.CantidadPersonas} personas)."
            });
        }
        var configuracionAlerta = await _context.ConfiguracionesReserva.FirstOrDefaultAsync();

        if (configuracionAlerta != null && configuracionAlerta.LimiteReservasPorDia > 0)
        {
            var reservasActuales = await _context.Reservas.CountAsync(r =>
                r.FechaHora.Date == fechaHora.Date &&
                (r.Estado == "Pendiente" || r.Estado == "Confirmada"));

            var totalConNuevaReserva = reservasActuales + 1;
            var umbralAlerta = (int)Math.Ceiling(configuracionAlerta.LimiteReservasPorDia * 0.80);

            if (totalConNuevaReserva == umbralAlerta)
            {
                foreach (var administradorId in administradoresIds)
                {
                    _context.Notificaciones.Add(new Notificacion
                    {
                        UsuarioId = administradorId,
                        Mensaje = $"Alerta: las reservas del {fechaHora:dd/MM/yyyy} alcanzaron el 80% del límite diario ({totalConNuevaReserva} de {configuracionAlerta.LimiteReservasPorDia})."
                    });
                }
            }
        }

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Solicitud enviada. Tu reserva está pendiente de confirmación por el administrador.";
        return RedirectToAction(nameof(MisReservas));
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerHorariosDisponibles(string fecha)
    {
        if (!DateTime.TryParse(fecha, out var fechaSeleccionada))
            return Json(new { error = "Fecha inválida." });

        var config = await _context.ConfiguracionesReserva.FirstOrDefaultAsync();

        if (config == null || !config.Activo)
            return Json(new { error = "Actualmente no se aceptan reservas." });

        if (fechaSeleccionada.Date < ObtenerHoraCostaRica().Date)
            return Json(new { error = "Seleccioná una fecha futura." });

        var nombreDia = ObtenerNombreDia(fechaSeleccionada.DayOfWeek);
        var diasCierre = ObtenerDiasCierre(config);

        if (diasCierre.Contains(nombreDia, StringComparer.OrdinalIgnoreCase))
            return Json(new { error = "El restaurante permanece cerrado ese día." });

        var reservasDelDia = await _context.Reservas
            .Where(r => r.FechaHora.Date == fechaSeleccionada.Date
                && r.Estado != "Cancelada"
                && r.Estado != "Rechazada")
            .Select(r => r.FechaHora)
            .ToListAsync();

    if (reservasDelDia.Count >= config.LimiteReservasPorDia)
    {
        return Json(new
        {
            error = "Ya se alcanzó el límite de reservas para ese día. Seleccioná otra fecha."
        });
    }

    var horarios = new List<string>();
        var horaActual = config.HoraApertura;

        while (horaActual < config.HoraCierre)
        {
            var fechaHoraSlot = fechaSeleccionada.Date.Add(horaActual);

            if (fechaHoraSlot > ObtenerHoraCostaRica() &&
                !reservasDelDia.Any(r => r.TimeOfDay == horaActual))
            {
                horarios.Add(horaActual.ToString(@"hh\:mm"));
            }

            horaActual = horaActual.Add(TimeSpan.FromMinutes(30));
        }

        return Json(horarios);
    }

    public async Task<IActionResult> MisReservas()
    {
        var usuarioId = ObtenerUsuarioId();

        var reservas = await _context.Reservas
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.FechaHora)
            .ToListAsync();

        return View(reservas);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        var usuarioId = ObtenerUsuarioId();
        var reserva = await _context.Reservas.FindAsync(id);

        if (reserva == null || reserva.UsuarioId != usuarioId)
        {
            TempData["Error"] = "Reserva no encontrada.";
            return RedirectToAction(nameof(MisReservas));
        }

        if (reserva.Estado != "Pendiente")
        {
            TempData["Error"] = "Solo se pueden cancelar reservas pendientes.";
            return RedirectToAction(nameof(MisReservas));
        }

        reserva.Estado = "Cancelada";

        var administradoresIds = await _context.Usuarios
            .Where(u => u.RolId == 1)
            .Select(u => u.UsuarioId)
            .ToListAsync();

        foreach (var administradorId in administradoresIds)
        {
            _context.Notificaciones.Add(new Notificacion
            {
                UsuarioId = administradorId,
                Mensaje = $"El cliente canceló la reserva {reserva.CodigoConfirmacion} para {reserva.FechaHora:dd/MM/yyyy HH:mm}."
            });
        }

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "La solicitud de reserva fue cancelada.";
        return RedirectToAction(nameof(MisReservas));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var usuarioId = ObtenerUsuarioId();

        var reserva = await _context.Reservas
            .FirstOrDefaultAsync(r => r.ReservaId == id && r.UsuarioId == usuarioId);

        if (reserva == null)
            return NotFound();

        if (reserva.Estado != "Pendiente")
        {
            TempData["Error"] = "Solo se pueden editar reservas pendientes.";
            return RedirectToAction(nameof(MisReservas));
        }

        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int reservaId, DateTime fechaHora, int cantidadPersonas, string? notas)
    {
        var usuarioId = ObtenerUsuarioId();

        var reserva = await _context.Reservas
            .FirstOrDefaultAsync(r => r.ReservaId == reservaId && r.UsuarioId == usuarioId);

        if (reserva == null)
            return NotFound();

        if (reserva.Estado != "Pendiente")
        {
            TempData["Error"] = "Solo se pueden editar reservas pendientes.";
            return RedirectToAction(nameof(MisReservas));
        }

        var error = await ValidarReservaAsync(fechaHora, cantidadPersonas, reservaId);

        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View(reserva);
        }

        var cambioReal = reserva.FechaHora != fechaHora ||
                         reserva.CantidadPersonas != cantidadPersonas ||
                         !string.Equals(reserva.Notas, notas?.Trim() ?? string.Empty, StringComparison.Ordinal);

        reserva.FechaHora = fechaHora;
        reserva.CantidadPersonas = cantidadPersonas;
        reserva.Notas = notas?.Trim() ?? string.Empty;

        if (cambioReal)
        {
            var administradoresIds = await _context.Usuarios
                .Where(u => u.RolId == 1)
                .Select(u => u.UsuarioId)
                .ToListAsync();

            foreach (var administradorId in administradoresIds)
            {
                _context.Notificaciones.Add(new Notificacion
                {
                    UsuarioId = administradorId,
                    Mensaje = $"El cliente modificó la reserva {reserva.CodigoConfirmacion}. Nueva fecha: {reserva.FechaHora:dd/MM/yyyy HH:mm}. Personas: {reserva.CantidadPersonas}."
                });
            }
        }

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Reserva actualizada correctamente.";
        return RedirectToAction(nameof(MisReservas));
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Gestion()
    {
        var reservas = await _context.Reservas
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.FechaCreacion)
            .ToListAsync();

        return View(reservas);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Confirmar(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);

        if (reserva == null)
        {
            TempData["Error"] = "No se encontró la reserva.";
            return RedirectToAction(nameof(Gestion));
        }

        if (reserva.Estado != "Pendiente")
        {
            TempData["Error"] = "Solo se pueden confirmar reservas pendientes.";
            return RedirectToAction(nameof(Gestion));
        }

        var conflicto = await _context.Reservas.AnyAsync(r =>
            r.ReservaId != id &&
            r.FechaHora == reserva.FechaHora &&
            (r.Estado == "Pendiente" || r.Estado == "Confirmada"));

        if (conflicto)
        {
            TempData["Error"] = "Ese horario ya tiene otra reserva pendiente o confirmada.";
            return RedirectToAction(nameof(Gestion));
        }

        reserva.Estado = "Confirmada";

        _context.Notificaciones.Add(new Notificacion
        {
            UsuarioId = reserva.UsuarioId,
            Mensaje = $"Tu reserva {reserva.CodigoConfirmacion} para el {reserva.FechaHora:dd/MM/yyyy HH:mm} ha sido confirmada."
        });
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"La reserva {reserva.CodigoConfirmacion} fue confirmada.";
        return RedirectToAction(nameof(Gestion));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Rechazar(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);

        if (reserva == null)
        {
            TempData["Error"] = "No se encontró la reserva.";
            return RedirectToAction(nameof(Gestion));
        }

        if (reserva.Estado != "Pendiente")
        {
            TempData["Error"] = "Solo se pueden rechazar reservas pendientes.";
            return RedirectToAction(nameof(Gestion));
        }

        reserva.Estado = "Rechazada";

        _context.Notificaciones.Add(new Notificacion
        {
            UsuarioId = reserva.UsuarioId,
            Mensaje = $"Tu reserva {reserva.CodigoConfirmacion} para el {reserva.FechaHora:dd/MM/yyyy HH:mm} ha sido rechazada."
        });
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = $"La reserva {reserva.CodigoConfirmacion} fue rechazada.";
        return RedirectToAction(nameof(Gestion));
    }

    [HttpGet]
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
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Configuracion(ConfiguracionReserva config)
    {
        if (config.LimiteReservasPorDia < 1)
            ModelState.AddModelError(nameof(config.LimiteReservasPorDia), "El límite debe ser mayor que cero.");

        if (config.HoraApertura >= config.HoraCierre)
            ModelState.AddModelError(nameof(config.HoraCierre), "La hora de cierre debe ser posterior a la apertura.");

        if (!ModelState.IsValid)
            return View(config);

        var existente = await _context.ConfiguracionesReserva
            .FirstOrDefaultAsync(c => c.ConfiguracionReservaId == config.ConfiguracionReservaId);

        var cambioHorario = existente != null &&
            (existente.HoraApertura != config.HoraApertura ||
             existente.HoraCierre != config.HoraCierre ||
             !string.Equals(existente.DiasCierre ?? string.Empty, config.DiasCierre ?? string.Empty, StringComparison.OrdinalIgnoreCase) ||
             existente.Activo != config.Activo);

        if (existente == null)
        {
            _context.ConfiguracionesReserva.Add(config);
        }
        else
        {
            existente.LimiteReservasPorDia = config.LimiteReservasPorDia;
            existente.HoraApertura = config.HoraApertura;
            existente.HoraCierre = config.HoraCierre;
            existente.DiasCierre = config.DiasCierre ?? string.Empty;
            existente.Activo = config.Activo;
        }

        if (cambioHorario)
        {
            var clientesIds = await _context.Usuarios
                .Where(u => u.RolId == 2)
                .Select(u => u.UsuarioId)
                .ToListAsync();

            foreach (var clienteId in clientesIds)
            {
                _context.Notificaciones.Add(new Notificacion
                {
                    UsuarioId = clienteId,
                    Mensaje = $"El horario de atención ha cambiado. Apertura: {config.HoraApertura}, cierre: {config.HoraCierre}, días de cierre: {config.DiasCierre}. Revisá los horarios antes de realizar tu reserva."
                });
            }
        }

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Configuración guardada correctamente.";
        return RedirectToAction(nameof(Configuracion));
    }

    private async Task<string?> ValidarReservaAsync(DateTime fechaHora, int cantidadPersonas, int? reservaIdExcluir = null)
    {
        if (fechaHora <= ObtenerHoraCostaRica())
            return "La fecha y hora deben ser futuras.";

        if (cantidadPersonas < 1 || cantidadPersonas > 20)
            return "La cantidad permitida es de 1 a 20 personas.";

        var config = await _context.ConfiguracionesReserva.FirstOrDefaultAsync();

        if (config == null || !config.Activo)
            return "Actualmente no estamos aceptando reservas.";

        var nombreDia = ObtenerNombreDia(fechaHora.DayOfWeek);
        var diasCierre = ObtenerDiasCierre(config);

        if (diasCierre.Contains(nombreDia, StringComparer.OrdinalIgnoreCase))
            return "El restaurante permanece cerrado ese día. Seleccioná otro día.";

        var hora = fechaHora.TimeOfDay;

        if (hora < config.HoraApertura || hora >= config.HoraCierre)
            return $"El horario permitido es de {config.HoraApertura:hh\\:mm} a {config.HoraCierre:hh\\:mm}.";

        var minutosDesdeApertura = (hora - config.HoraApertura).TotalMinutes;

        if (minutosDesdeApertura % 30 != 0)
            return "Solo se permiten reservas en intervalos de 30 minutos.";

        var reservasDelDia = _context.Reservas.Where(r =>
            r.FechaHora.Date == fechaHora.Date &&
            r.Estado != "Cancelada" &&
            r.Estado != "Rechazada");

        if (reservaIdExcluir.HasValue)
            reservasDelDia = reservasDelDia.Where(r => r.ReservaId != reservaIdExcluir.Value);

        var cantidadReservas = await reservasDelDia.CountAsync();

        if (cantidadReservas >= config.LimiteReservasPorDia)
            return "Ya se alcanzó el límite de reservas para ese día.";

        var horarioOcupado = await _context.Reservas.AnyAsync(r =>
            r.ReservaId != (reservaIdExcluir ?? 0) &&
            r.FechaHora == fechaHora &&
            (r.Estado == "Pendiente" || r.Estado == "Confirmada"));

        if (horarioOcupado)
            return "Ese horario ya está reservado. Seleccioná otro horario.";

        return null;
    }

    private static DateTime ObtenerHoraCostaRica()
{
    var zonaHoraria = TimeZoneInfo.FindSystemTimeZoneById(
        "Central America Standard Time"
    );

    return TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow,
        zonaHoraria
    );
}

private int ObtenerUsuarioId()
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : 0;
    }

    private static string ObtenerNombreDia(DayOfWeek dia)
    {
        return dia switch
        {
            DayOfWeek.Monday => "Lunes",
            DayOfWeek.Tuesday => "Martes",
            DayOfWeek.Wednesday => "Miércoles",
            DayOfWeek.Thursday => "Jueves",
            DayOfWeek.Friday => "Viernes",
            DayOfWeek.Saturday => "Sábado",
            DayOfWeek.Sunday => "Domingo",
            _ => string.Empty
        };
    }

    private static List<string> ObtenerDiasCierre(ConfiguracionReserva config)
    {
        return string.IsNullOrWhiteSpace(config.DiasCierre)
            ? new List<string>()
            : config.DiasCierre.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
    }
}



