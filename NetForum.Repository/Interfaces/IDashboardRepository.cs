namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface IDashboardRepository
{
    Task<int> ContarUsuariosAsync();

    Task<int> ContarTemasAsync();

    Task<int> ContarRespuestasAsync();

    Task<int> ContarEquiposAsync();

    Task<int> ContarLigasAsync();
}