using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetForum.Business.Interfaces;
using NetForum.MVC.Models;

namespace NetForum.MVC.Controllers;

[Authorize]
public class ForoController : Controller
{
    private readonly ITemaService _temaService;

    public ForoController(ITemaService temaService)
    {
        _temaService = temaService;
    }

    [HttpGet] //Método que obtiene la lista de temas del foro, con opciones de búsqueda y filtrado por categoría.
    public async Task<IActionResult> Index(
        string? buscar,
        int? categoriaId)
    {
        var temas = await _temaService.ObtenerFeedAsync(
            buscar,
            categoriaId);

        var categorias =
            await _temaService.ObtenerCategoriasAsync();

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
                    ContenidoResumen =
                        CrearResumen(t.Contenido, 220),
                    Autor = t.Usuario.NombreUsuario,
                    Categoria = t.Categoria.Nombre,
                    Estado = t.Estado.Nombre,
                    FechaCreacion = t.FechaCreacion,
                    CantidadRespuestas =
                        t.Respuestas?.Count ?? 0,
                    CantidadReacciones =
                        t.Reacciones?.Count ?? 0
                })
        };

        return View(viewModel);
    }

    [HttpGet]  //Método que obtiene los detalles de un tema específico en el foro.
               //Se espera un parámetro 'id' que representa el identificador del tema.
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
}