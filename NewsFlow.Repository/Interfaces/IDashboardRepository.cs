using NewsFlow.Data.Entities;

namespace NewsFlow.Repository.Interfaces;

// SOLID: Single Responsibility Principle - encapsula únicamente las consultas de
// conteo para el dashboard. No mezcla lógica de otras entidades ni de otros repositorios
public interface IDashboardRepository
{
    Task<int> ContarUsuariosAsync();
    Task<int> ContarTemasAsync();
    Task<int> ContarRespuestasAsync();
    Task<int> ContarEquiposAsync();
    Task<int> ContarLigasAsync();
    Task<IEnumerable<Tema>> ObtenerTemasDestacadosAsync();
    Task<Dictionary<string, int>> ObtenerTemasPorCategoriaAsync();
    Task<Dictionary<DateTime, int>> ObtenerRespuestasPorDiaAsync(
    int dias = 7);
}