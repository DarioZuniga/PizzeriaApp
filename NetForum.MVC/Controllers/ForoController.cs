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

    [HttpGet]
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