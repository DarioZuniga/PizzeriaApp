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
    // SOLID: Dependency Inversion Principle - el controller depende de abstracciones
    private readonly ITemaService _temaService; // Inyección de dependencias para el servicio de temas del foro.
    private readonly IRespuestaService _respuestaService; // Inyección de dependencias para el servicio de respuestas del foro.
    private readonly IReporteService _reporteService; // Inyección de dependencias para el servicio de reportes del foro.
    private readonly IReaccionService _reaccionService; // Inyección de dependencias para el servicio de reacciones del foro.
    private readonly ITemaEtiquetaService _temaEtiquetaService; // Inyección de dependencias para el
                                                                // servicio de etiquetas de temas del foro.
    public ForoController( // Controller, su finción es manejar las solicitudes HTTP
                           // relacionadas con el foro, como mostrar la lista de temas,
                           // los detalles de un tema específico, crear respuestas, reportar temas y reaccionar a ellos.
      ITemaService temaService,
      IRespuestaService respuestaService,
      IReporteService reporteService,
      IReaccionService reaccionService,
      ITemaEtiquetaService temaEtiquetaService)
    {
        _temaService = temaService; 
        _respuestaService = respuestaService;
        _reporteService = reporteService;
        _reaccionService = reaccionService;
        _temaEtiquetaService = temaEtiquetaService;
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
    [HttpGet]
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

        var tiposReaccion =
            await _reaccionService.ObtenerTiposAsync();

        var reaccionesTema =
            await _reaccionService.ObtenerPorTemaAsync(id);

        var usuarioIdTexto =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        int.TryParse(usuarioIdTexto, out var usuarioId);

        var viewModel = new ForoDetalleViewModel
        {
            TemaId = tema.TemaId,
            Titulo = tema.Titulo,
            Contenido = tema.Contenido,
            Autor = tema.Usuario.NombreUsuario,
            Categoria = tema.Categoria.Nombre,
            Estado = tema.Estado.Nombre,
            FechaCreacion = tema.FechaCreacion,
            CantidadReacciones = reaccionesTema.Count(),

            Reacciones = tiposReaccion.Select(tipo =>
    new ForoTipoReaccionViewModel
    {
        TipoReaccionId = tipo.TipoReaccionId,
        Nombre = tipo.Nombre,

        Cantidad = reaccionesTema.Count(r =>
            r.TipoReaccionId == tipo.TipoReaccionId),

        SeleccionadaPorUsuario =
            usuarioId > 0 &&
            reaccionesTema.Any(r =>
                r.TipoReaccionId == tipo.TipoReaccionId &&
                r.UsuarioId == usuarioId)
    }),

            Respuestas = tema.Respuestas
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
    public async Task<IActionResult> ResponderAjax( // acción AJAX para crear una nueva respuesta en un tema del foro, que recibe los datos de la respuesta y
                                                    // los valida antes de enviarlos al servicio correspondiente
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

        var usuarioIdTexto = User.FindFirstValue( // obtiene el identificador del usuario autenticado desde
                                                  // los claims del token de autenticación
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
            await _respuestaService.CrearRespuestaForoAsync( // llama al servicio de respuestas para crear
                                                             // una nueva respuesta en el tema del foro
                viewModel.TemaId,
                usuarioId,
                viewModel.Contenido);

            return Json(new
            {
                success = true,
                message = "Tu respuesta fue publicada correctamente."
            });
        }
        catch (ArgumentException ex) // captura excepciones de argumento y devuelve un mensaje de error
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex) // captura excepciones de operación inválida y devuelve un mensaje de error
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
    public async Task<IActionResult> Crear() // acción AJAX para crear un nuevo tema en el foro,
                                             // que obtiene las categorías disponibles
    {
        var categorias =
            await _temaService.ObtenerCategoriasAsync();

        var etiquetas =
            await _temaEtiquetaService.ObtenerEtiquetasAsync();

        ViewBag.Categorias = categorias
            .Select(c => new SelectListItem
            {
                Value = c.CategoriaId.ToString(),
                Text = c.Nombre
            })
            .ToList();

        ViewBag.Etiquetas = etiquetas;

        return View(new CrearTemaForoViewModel());
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearAjax( // acción AJAX para crear un nuevo tema en el foro, que recibe los datos
                                                // del tema y los valida antes de enviarlos al servicio correspondiente
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
            var tema = await _temaService.CrearTemaForoAsync( // llama al servicio de temas para crear un nuevo tema en el foro
                usuarioId,
                viewModel.CategoriaId,
                viewModel.Titulo,
                viewModel.Contenido);

            await _temaEtiquetaService.ReemplazarEtiquetasAsync( // llama al servicio de etiquetas de temas
                                                                 // para reemplazar las etiquetas asociadas al tema recién creado
                    tema.TemaId,
                    viewModel.EtiquetaIds);

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReaccionarAjax(
    int temaId,
    int tipoReaccionId)
    {
        var usuarioIdTexto = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdTexto, out var usuarioId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "No fue posible identificar al usuario."
            });
        }

        try
        {
            var resultado =
                await _reaccionService.AlternarReaccionAsync(
                    usuarioId,
                    temaId,
                    tipoReaccionId);

            return Json(new
            {
                success = true,
                agregada = resultado.Agregada,
                message = resultado.Mensaje,
                totalReacciones = resultado.TotalReacciones
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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
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