using PizzeriaApp.Business.Models.Football; // using directive for the FootballMatchDto class

namespace PizzeriaApp.Business.Interfaces;

public interface IFootballApiService // interface for the football API service
{
    Task<IEnumerable<FootballMatchDto>> ObtenerProximosPartidosAsync( // metodo para obtener los pr�ximos partidos de f�tbol
        int cantidad = 5);
    Task<IEnumerable<FootballMatchDto>> ObtenerUltimosResultadosAsync(
    int cantidad = 5);
}