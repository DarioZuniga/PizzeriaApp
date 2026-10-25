
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle - agrupa �nicamente las consultas de conteo que alimentan el Dashboard.
public interface IDashboardService
{
    Task<int> ContarUsuariosAsync();

    Task<int> ContarTemasAsync();

    Task<int> ContarRespuestasAsync();

    Task<int> ContarEquiposAsync();

    Task<int> ContarLigasAsync();
    Task<IEnumerable<Tema>> ObtenerTemasDestacadosAsync();

    Task<Dictionary<string, int>> ObtenerTemasPorCategoriaAsync();

    Task<Dictionary<DateTime, int>>
    ObtenerRespuestasPorDiaAsync(
        int dias = 7);
}