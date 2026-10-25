namespace PizzeriaApp.MVC.Models;

// SOLID: Single Responsibility Principle
// Este ViewModel �nicamente transporta informaci�n hacia la vista.

public class DashboardViewModel
{
    public int TotalUsuarios { get; set; }

    public int TotalTemas { get; set; }

    public int TotalRespuestas { get; set; }

    public int TotalEquipos { get; set; }

    public int TotalLigas { get; set; }

    public string NombreUsuario { get; set; } = string.Empty;

    public IEnumerable<TemaDestacadoViewModel> TemasDestacados { get; set; } // funci�n: transporta la informaci�n de los
                                                                             // temas destacados hacia la vista
        = Enumerable.Empty<TemaDestacadoViewModel>();

    public IEnumerable<PartidoProximoViewModel> ProximosPartidos { get; set; } // funci�n: transporta la informaci�n
                                                                               // de los pr�ximos partidos hacia la vista
    = Enumerable.Empty<PartidoProximoViewModel>();

    public IEnumerable<GraficoTemasCategoriaViewModel> TemasPorCategoria  { get; set; } // funci�n: transporta la informaci�n de los
                                                                                        // temas por categor�a hacia la vista
        = Enumerable.Empty<GraficoTemasCategoriaViewModel>();

    public IEnumerable<GraficoRespuestasDiaViewModel> 
    RespuestasPorDia
    { get; set; }
        = Enumerable.Empty<GraficoRespuestasDiaViewModel>();

    public IEnumerable<ResultadoRecienteViewModel>
    UltimosResultados
    { get; set; }
        = Enumerable.Empty<ResultadoRecienteViewModel>();
}