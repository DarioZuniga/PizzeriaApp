using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using System.Security.Claims;

namespace PizzeriaApp.MVC.Controllers;

[Authorize]
public class EvaluacionController : Controller
{
    private readonly PizzeriaAppDbContext _context;
    public EvaluacionController(PizzeriaAppDbContext context) { _context = context; }

    private int GetUsuarioId() =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    // ===== CLIENTE: Crear evaluación =====
    [HttpGet]
    public async Task<IActionResult> Crear(int pedidoId)
    {
        var usuarioId = GetUsuarioId();
        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId && p.UsuarioId == usuarioId);
        if (pedido == null)
        {
            TempData["Error"] = "Pedido no encontrado.";
            return RedirectToAction("Historial", "Pedido");
        }

        var yaEvaluado = await _context.Evaluaciones.AnyAsync(e => e.PedidoId == pedidoId);
        if (yaEvaluado)
        {
            TempData["Error"] = "Ya calificaste este pedido.";
            return RedirectToAction("Historial", "Pedido");
        }

        ViewBag.PedidoId = pedidoId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(int pedidoId, int calificacion, string comentario)
    {
        var usuarioId = GetUsuarioId();

        if (calificacion < 1 || calificacion > 10)
        {
            ModelState.AddModelError("", "La calificación debe estar entre 1 y 10.");
            ViewBag.PedidoId = pedidoId;
            return View();
        }

        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(p => p.PedidoId == pedidoId && p.UsuarioId == usuarioId);
        if (pedido == null)
        {
            TempData["Error"] = "Pedido no encontrado.";
            return RedirectToAction("Historial", "Pedido");
        }

        var yaEvaluado = await _context.Evaluaciones.AnyAsync(e => e.PedidoId == pedidoId);
        if (yaEvaluado)
        {
            TempData["Error"] = "Ya calificaste este pedido.";
            return RedirectToAction("Historial", "Pedido");
        }

        var evaluacion = new Evaluacion
        {
            PedidoId = pedidoId,
            UsuarioId = usuarioId,
            Calificacion = calificacion,
            Comentario = comentario ?? string.Empty,
            Aprobada = false,
            Fecha = DateTime.Now
        };

        _context.Evaluaciones.Add(evaluacion);
        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "¡Gracias por tu evaluación! Será revisada antes de publicarse.";
        return RedirectToAction("Historial", "Pedido");
    }

    // ===== ADMIN: Panel de moderación =====
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Index(string filtro = "todos")
    {
        var query = _context.Evaluaciones
            .Include(e => e.Pedido)
                .ThenInclude(p => p.Usuario)
            .AsQueryable();

        if (filtro == "pendientes") query = query.Where(e => !e.Aprobada);
        else if (filtro == "aprobadas") query = query.Where(e => e.Aprobada);

        var lista = await query.OrderByDescending(e => e.Fecha).ToListAsync();

        // Stats para el panel
        var todas = await _context.Evaluaciones.ToListAsync();
        ViewBag.Total = todas.Count;
        ViewBag.Pendientes = todas.Count(e => !e.Aprobada);
        ViewBag.Aprobadas = todas.Count(e => e.Aprobada);
        ViewBag.Promedio = todas.Any() ? todas.Average(e => e.Calificacion) : 0;
        ViewBag.Filtro = filtro;

        return View(lista);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Aprobar(int id)
    {
        var ev = await _context.Evaluaciones.FindAsync(id);
        if (ev != null)
        {
            ev.Aprobada = true;
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Evaluación aprobada y visible en el Home.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rechazar(int id)
    {
        var ev = await _context.Evaluaciones.FindAsync(id);
        if (ev != null)
        {
            _context.Evaluaciones.Remove(ev);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Evaluación eliminada.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Responder(int id, string respuesta)
    {
        var ev = await _context.Evaluaciones.FindAsync(id);
        if (ev != null)
        {
            if (string.IsNullOrWhiteSpace(respuesta))
            {
                TempData["Error"] = "La respuesta no puede estar vacía.";
            }
            else
            {
                ev.RespuestaAdmin = respuesta.Trim();
                ev.FechaRespuesta = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Respuesta guardada. Se mostrará junto al comentario.";
            }
        }
        return RedirectToAction(nameof(Index));
    }

    // ===== ADMIN: Reporte =====
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Reporte()
    {
        var todas = await _context.Evaluaciones.ToListAsync();

        ViewBag.Total = todas.Count;
        ViewBag.Promedio = todas.Any() ? todas.Average(e => e.Calificacion) : 0;
        ViewBag.Aprobadas = todas.Count(e => e.Aprobada);
        ViewBag.Pendientes = todas.Count(e => !e.Aprobada);
        ViewBag.ConRespuesta = todas.Count(e => !string.IsNullOrWhiteSpace(e.RespuestaAdmin));

        // Distribución por calificación (1-10)
        var distribucion = new Dictionary<int, int>();
        for (int i = 1; i <= 10; i++)
            distribucion[i] = todas.Count(e => e.Calificacion == i);
        ViewBag.Distribucion = distribucion;

        // Palabras clave más mencionadas (mínimo 4 letras, stop words filtradas)
        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "para", "como", "pero", "esta", "este", "esto", "muy", "mas", "más",
            "con", "por", "sin", "que", "los", "las", "del", "una", "uno", "unos",
            "fue", "son", "era", "sus", "les", "nos", "todo", "toda", "todos",
            "pizza", "pizzas", "pedido", "pedidos", "servicio", "muy", "bien"
        };

        var palabras = todas
            .Where(e => !string.IsNullOrWhiteSpace(e.Comentario))
            .SelectMany(e => e.Comentario.ToLower()
                .Split(new[] { ' ', ',', '.', ';', ':', '!', '?', '¡', '¿', '\n', '\r', '\t' },
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => p.Length >= 4 && !stopWords.Contains(p)))
            .GroupBy(p => p)
            .OrderByDescending(g => g.Count())
            .Take(20)
            .Select(g => new { Palabra = g.Key, Cantidad = g.Count() })
            .ToList();

        ViewBag.PalabrasClave = palabras;

        return View(todas);
    }
}