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
    private readonly IFootballApiService _footballApiService; // función: servicio para obtener los próximos partidos
                                                              // de fútbol desde una API externa

       public HomeController(   // constructor del controlador que recibe las dependencias a través de inyección
                                // de dependencias
      IDashboardService dashboardService,  // función: servicio para obtener los datos del dashboard desde la capa de negocio
      IFootballApiService footballApiService) // función: servicio para obtener los próximos partidos de fútbol desde una API externa
    {
        _dashboardService = dashboardService; 
        _footballApiService = footballApiService; 
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
        var proximosPartidos =   // API externa para obtener los próximos partidos de fútbol
                 await _footballApiService
                    .ObtenerProximosPartidosAsync(5);
        var temasPorCategoria =    // función : obtiene la cantidad de temas por categoría desde la capa de negocio
              await _dashboardService
                 .ObtenerTemasPorCategoriaAsync();

        var model = new DashboardViewModel
        {
            TemasPorCategoria = temasPorCategoria
                  .Select(x => new GraficoTemasCategoriaViewModel // proyección de datos (mapeo) mediante LINQ,
                                                                  // lo que permite transformar los datos obtenidos
                                                                  // de la capa de negocio en objetos listos
                                                                  // para mostrar en la vista
                  {
                     Categoria = x.Key,
                        Cantidad = x.Value
                      })
                     .ToList(),

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
                .ToList(),

            /*proyección de datos (mapeo) mediante LINQ para convertir los partidos obtenidos de la API en una lista de objetos PartidoProximoViewModel listos para mostrar en la interfaz.

        Puntos clave de lo que hace:

    Proyección con Select: Transforma cada objeto partido (p) de la lista de origen en un nuevo PartidoProximoViewModel.

    Lógica de nombres (Fallback): Evalúa si el equipo tiene un nombre corto (ShortName). Si existe y no está vacío, lo usa; de lo contrario, utiliza el nombre completo (Name).

    Mapeo de atributos: Copia los escudos (Crest), la competición y la fecha en formato UTC.

    Materialización con ToList(): Ejecuta la consulta de LINQ y asigna la lista final a la propiedad ProximosPartidos.*/

            ProximosPartidos = proximosPartidos
                 .Select(p => new PartidoProximoViewModel
    {
                  PartidoId = p.Id,
                  Competicion = p.Competition.Name,

                  Local = string.IsNullOrWhiteSpace(
                         p.HomeTeam.ShortName)
                            ? p.HomeTeam.Name
                           : p.HomeTeam.ShortName,

                           Visitante = string.IsNullOrWhiteSpace(
                         p.AwayTeam.ShortName)
                           ? p.AwayTeam.Name
                           : p.AwayTeam.ShortName,
        
                          EscudoLocal = p.HomeTeam.Crest,

                       EscudoVisitante = p.AwayTeam.Crest,

                       FechaUtc = p.UtcDate
    })
                     .ToList(),
                       };

                         return View(model);
    }
}