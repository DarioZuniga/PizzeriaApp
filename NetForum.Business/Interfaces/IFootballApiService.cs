using NetForum.Business.Models.Football; // using directive for the FootballMatchDto class

namespace NetForum.Business.Interfaces;

public interface IFootballApiService // interface for the football API service
{
    Task<IEnumerable<FootballMatchDto>> ObtenerProximosPartidosAsync( // metodo para obtener los próximos partidos de fútbol
        int cantidad = 5);
    Task<IEnumerable<FootballMatchDto>> ObtenerUltimosResultadosAsync(
    int cantidad = 5);
}