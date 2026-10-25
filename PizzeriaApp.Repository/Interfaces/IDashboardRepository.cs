using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// SOLID: Single Responsibility Principle - encapsula �nicamente las consultas de
// conteo para el dashboard. No mezcla l�gica de otras entidades ni de otros repositorios
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