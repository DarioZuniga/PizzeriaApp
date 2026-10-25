using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.MVC.Models;

namespace PizzeriaApp.MVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de la abstracción
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    private static string CrearResumen(string contenido, int longitudMaxima)
    {
        if (string.IsNullOrWhiteSpace(contenido))
        {
            return string.Empty;
        }

        var limpio = contenido.Trim();

        if (limpio.Length <= longitudMaxima)
        {
            return limpio;
        }

        return limpio[..longitudMaxima] + "...";
    }

    private static string CrearIniciales(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            return "NF";
        }

        var palabras = titulo
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Take(2)
            .ToArray();

        var iniciales = string.Concat(palabras.Select(p => char.ToUpperInvariant(p[0])));

        return string.IsNullOrWhiteSpace(iniciales) ? "NF" : iniciales;
    }

    public async Task<IActionResult> Index()
    {
        var temasDestacados = await _dashboardService.ObtenerTemasDestacadosAsync();
        var temasPorCategoria = await _dashboardService.ObtenerTemasPorCategoriaAsync();
        var respuestasPorDia = await _dashboardService.ObtenerRespuestasPorDiaAsync(7);

        var model = new DashboardViewModel
        {
            RespuestasPorDia = respuestasPorDia
                .Select(x => new GraficoRespuestasDiaViewModel
                {
                    Fecha = x.Key,
                    Cantidad = x.Value
                })
                .OrderBy(x => x.Fecha)
                .ToList(),

            TemasPorCategoria = temasPorCategoria
                .Select(x => new GraficoTemasCategoriaViewModel
                {
                    Categoria = x.Key,
                    Cantidad = x.Value
                })
                .ToList(),

            NombreUsuario = User.Identity?.Name ?? "Futbolero",

            TotalUsuarios = await _dashboardService.ContarUsuariosAsync(),
            TotalTemas = await _dashboardService.ContarTemasAsync(),
            TotalRespuestas = await _dashboardService.ContarRespuestasAsync(),
            TotalEquipos = await _dashboardService.ContarEquiposAsync(),
            TotalLigas = await _dashboardService.ContarLigasAsync(),

            TemasDestacados = temasDestacados
                .Select(t => new TemaDestacadoViewModel
                {
                    TemaId = t.TemaId,
                    Titulo = t.Titulo,
                    Resumen = CrearResumen(t.Contenido, 120),
                    Iniciales = CrearIniciales(t.Titulo),
                    CantidadRespuestas = t.Respuestas.Count,
                    CantidadReacciones = t.Reacciones.Count,
                    FechaCreacion = t.FechaCreacion
                })
                .ToList(),

            // ❌ ProximosPartidos y UltimosResultados eliminados (eran de fútbol)
        };

        return View(model);
    }
}