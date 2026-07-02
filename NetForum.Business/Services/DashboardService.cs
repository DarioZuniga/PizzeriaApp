using NetForum.Business.Interfaces;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
// Esta clase centraliza la lógica de negocio del Dashboard.
public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        // SOLID: Dependency Inversion Principle
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
}