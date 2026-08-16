using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracción
// IDashboardRepository, no de una implementación concreta.
public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<int> ContarUsuariosAsync()
    {
        return await _dashboardRepository.ContarUsuariosAsync();
    }

    public async Task<int> ContarTemasAsync()
    {
        return await _dashboardRepository.ContarTemasAsync();
    }

    public async Task<int> ContarRespuestasAsync()
    {
        return await _dashboardRepository.ContarRespuestasAsync();
    }

    public async Task<int> ContarEquiposAsync()
    {
        return await _dashboardRepository.ContarEquiposAsync();
    }

    public async Task<int> ContarLigasAsync() 
    {
        return await _dashboardRepository.ContarLigasAsync();
    }
    public async Task<IEnumerable<Tema>> ObtenerTemasDestacadosAsync() // DP: Service Layer Pattern
    {
        return await _dashboardRepository.ObtenerTemasDestacadosAsync();
    }
}