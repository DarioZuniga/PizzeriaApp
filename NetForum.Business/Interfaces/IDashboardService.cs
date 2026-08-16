
using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle - agrupa únicamente las consultas de conteo que alimentan el Dashboard.
public interface IDashboardService
{
    Task<int> ContarUsuariosAsync();

    Task<int> ContarTemasAsync();

    Task<int> ContarRespuestasAsync();

    Task<int> ContarEquiposAsync();

    Task<int> ContarLigasAsync();
    Task<IEnumerable<Tema>> ObtenerTemasDestacadosAsync();
}