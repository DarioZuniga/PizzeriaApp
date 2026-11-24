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

        reserva.FechaHora = fechaHora;
        reserva.CantidadPersonas = cantidadPersonas;
        reserva.Notas = notas?.Trim() ?? string.Empty;

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



