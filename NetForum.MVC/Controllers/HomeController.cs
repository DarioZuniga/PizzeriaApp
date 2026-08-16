using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetForum.Business.Interfaces;
using NetForum.MVC.Models;

namespace NetForum.MVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de la abstracción
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService) // DP: Service Layer Pattern
    {
        _dashboardService = dashboardService; // SOLID: Dependency Inversion Principle - inyección de dependencias
    }
              
    private static string CrearResumen( // método privado para crear un resumen del contenido de un tema,
                                        // limitando la longitud máxima
    string contenido,
    int longitudMaxima)
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

    private static string CrearIniciales(string titulo) // método privado para crear las iniciales de un título,
                                                        // tomando las primeras letras de las primeras dos palabras
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            return "NF";
        }

        var palabras = titulo
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries)
            .Take(2)
            .ToArray();

        var iniciales = string.Concat(
            palabras.Select(p =>
                char.ToUpperInvariant(p[0])));

        return string.IsNullOrWhiteSpace(iniciales)
            ? "NF"
            : iniciales;
    }
    

    public async Task<IActionResult> Index() // acción del controlador que obtiene los datos del dashboard y
                                             // los pasa a la vista
    {
        var temasDestacados =
            await _dashboardService.ObtenerTemasDestacadosAsync(); // DP: Service Layer Pattern

        var model = new DashboardViewModel
        {
            NombreUsuario =
                User.Identity?.Name ?? "Futbolero",

            TotalUsuarios =
                await _dashboardService.ContarUsuariosAsync(),

            TotalTemas =
                await _dashboardService.ContarTemasAsync(),

            TotalRespuestas =
                await _dashboardService.ContarRespuestasAsync(),

            TotalEquipos =
                await _dashboardService.ContarEquiposAsync(),

            TotalLigas =
                await _dashboardService.ContarLigasAsync(),

            TemasDestacados = temasDestacados
                .Select(t => new TemaDestacadoViewModel
                {
                    TemaId = t.TemaId,

                    Titulo = t.Titulo,

                    Resumen = CrearResumen(
                        t.Contenido,
                        120),

                    Iniciales = CrearIniciales(
                        t.Titulo),

                    CantidadRespuestas =
                        t.Respuestas.Count,

                    CantidadReacciones =
                        t.Reacciones.Count,

                    FechaCreacion =
                        t.FechaCreacion
                })
                .ToList()
        };

        return View(model);
    }
}