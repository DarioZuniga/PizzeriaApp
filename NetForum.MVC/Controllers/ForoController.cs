using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetForum.Business.Interfaces;
using NetForum.MVC.Models;
using System.Security.Claims;

namespace NetForum.MVC.Controllers;

[Authorize]
public class ForoController : Controller
{
    private readonly ITemaService _temaService; // Inyección de dependencias para el servicio de temas del foro.
    private readonly IRespuestaService _respuestaService; // Inyección de dependencias para el servicio de respuestas del foro.
    private readonly IReporteService _reporteService; // Inyección de dependencias para el servicio de reportes del foro.

    public ForoController(
      ITemaService temaService,
      IRespuestaService respuestaService,
      IReporteService reporteService)
    {
        _temaService = temaService;
        _respuestaService = respuestaService;
        _reporteService = reporteService;
    }

    [HttpGet] // Método que obtiene la lista de temas del foro, con opciones de búsqueda y filtrado por categoría.
    public async Task<IActionResult> Index(
        string? buscar,
        int? categoriaId)
    {
        var temas = await _temaService.ObtenerFeedAsync(
            buscar,
            categoriaId);

        var categorias = await _temaService.ObtenerCategoriasAsync();

        var viewModel = new ForoIndexViewModel
        {
            Buscar = buscar,
            CategoriaId = categoriaId,

            Categorias = categorias.Select(c =>
                new SelectListItem
                {
                    Value = c.CategoriaId.ToString(),
                    Text = c.Nombre,
                    Selected = c.CategoriaId == categoriaId
                }),

            Temas = temas.Select(t =>
                new ForoTemaViewModel
                {
                    TemaId = t.TemaId,
                    Titulo = t.Titulo,
                    ContenidoResumen = CrearResumen(t.Contenido, 220),
                    Autor = t.Usuario.NombreUsuario,
                    Categoria = t.Categoria.Nombre,
                    Estado = t.Estado.Nombre,
                    FechaCreacion = t.FechaCreacion,
                    CantidadRespuestas = t.Respuestas?.Count ?? 0,
                    CantidadReacciones = t.Reacciones?.Count ?? 0
                })
        };

        return View(viewModel);
    }

    [HttpGet] // Método que obtiene los detalles de un tema específico en el foro.
              // Se espera un parámetro 'id' que representa el identificador del tema.
    public async Task<IActionResult> Detalle(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }

        var tema = await _temaService.ObtenerDetalleAsync(id);

        if (tema is null)
        {
            return NotFound();
        }

        var viewModel = new ForoDetalleViewModel
        {
            TemaId = tema.TemaId,
            Titulo = tema.Titulo,
            Contenido = tema.Contenido,
            Autor = tema.Usuario.NombreUsuario,
            Categoria = tema.Categoria.Nombre,
            Estado = tema.Estado.Nombre,
            FechaCreacion = tema.FechaCreacion,
            CantidadReacciones = tema.Reacciones?.Count ?? 0,

            Respuestas = tema.Respuestas?
                .OrderBy(r => r.FechaCreacion)
                .Select(r => new ForoRespuestaViewModel
                {
                    RespuestaId = r.RespuestaId,
                    Contenido = r.Contenido,
                    Autor = r.Usuario.NombreUsuario,
                    Estado = r.Estado.Nombre,
                    FechaCreacion = r.FechaCreacion,
                    EsAutorTema = r.UsuarioId == tema.UsuarioId
                })
                .ToList()
                ?? new List<ForoRespuestaViewModel>()
        };

        return View(viewModel);
    }

    // Método privado que crea un resumen del contenido de un tema
    private static string CrearResumen(
        string contenido,
        int longitudMaxima)
    {
        if (string.IsNullOrWhiteSpace(contenido))
        {
            return string.Empty;
        }

        var contenidoLimpio = contenido.Trim();

        if (contenidoLimpio.Length <= longitudMaxima)
        {
            return contenidoLimpio;
        }

        return contenidoLimpio[..longitudMaxima] + "...";
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResponderAjax(
    CrearRespuestaViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var mensaje = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

            return BadRequest(new
            {
                success = false,
                message = mensaje ?? "Los datos enviados no son válidos."
            });
        }

        var usuarioIdTexto = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdTexto, out var usuarioId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "No fue posible identificar al usuario autenticado."
            });
        }

        var tema = await _temaService.ObtenerDetalleAsync(
            viewModel.TemaId);

        if (tema is null)
        {
            return NotFound(new
            {
                success = false,
                message = "El tema no existe."
            });
        }

        try
        {
            await _respuestaService.CrearRespuestaForoAsync(
                viewModel.TemaId,
                usuarioId,
                viewModel.Contenido);

            return Json(new
            {
                success = true,
                message = "Tu respuesta fue publicada correctamente."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
    // acción AJAX para crear un nuevo tema en el foro, que obtiene las categorías disponibles
    // y muestra un formulario para crear el tema.
    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var categorias =
            await _temaService.ObtenerCategoriasAsync();

        ViewBag.Categorias = categorias
            .Select(c => new SelectListItem
            {
                Value = c.CategoriaId.ToString(),
                Text = c.Nombre
            })
            .ToList();

        return View(new CrearTemaForoViewModel());
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearAjax(
    CrearTemaForoViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var mensaje = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

            return BadRequest(new
            {
                success = false,
                message = mensaje ?? "Los datos enviados no son válidos."
            });
        }

        var usuarioIdTexto = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdTexto, out var usuarioId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "No fue posible identificar al usuario autenticado."
            });
        }

        try
        {
            var tema = await _temaService.CrearTemaForoAsync(
                usuarioId,
                viewModel.CategoriaId,
                viewModel.Titulo,
                viewModel.Contenido);

            return Json(new
            {
                success = true,
                message = "El tema fue publicado correctamente.",
                temaId = tema.TemaId
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken] // acción AJAX para reportar un tema en el foro, que recibe los datos del reporte y
                               // los valida antes de enviarlos al servicio correspondiente
    public async Task<IActionResult> ReportarAjax(
    CrearReporteViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var mensaje = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();

            return BadRequest(new
            {
                success = false,
                message = mensaje ?? "Los datos enviados no son válidos."
            });
        }

        var usuarioIdTexto = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdTexto, out var usuarioId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "No fue posible identificar al usuario autenticado."
            });
        }

        var tema = await _temaService.ObtenerDetalleAsync(
            viewModel.TemaId);

        if (tema is null)
        {
            return NotFound(new
            {
                success = false,
                message = "El tema no existe."
            });
        }

        try
        {
            await _reporteService.CrearReporteForoAsync(
                viewModel.TemaId,
                usuarioId,
                viewModel.Motivo);

            return Json(new
            {
                success = true,
                message = "El reporte fue enviado correctamente."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

}