

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface IDashboardService
{
    Task<int> ContarUsuariosAsync();

    Task<int> ContarTemasAsync();

    Task<int> ContarRespuestasAsync();

    Task<int> ContarEquiposAsync();

    Task<int> ContarLigasAsync();
}