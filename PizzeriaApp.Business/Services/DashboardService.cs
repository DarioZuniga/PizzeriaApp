using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracci�n
// IDashboardRepository, no de una implementaci�n concreta.
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
    public async Task<Dictionary<string, int>> // m�todo para obtener la cantidad de temas por categor�a en el dashboard
    ObtenerTemasPorCategoriaAsync()
    {
        return await _dashboardRepository
            .ObtenerTemasPorCategoriaAsync();
    }

    public async Task<Dictionary<DateTime, int>> ObtenerRespuestasPorDiaAsync(  // metodo para obtener la cantidad
                                                                                // de respuestas por d�a en el dashboard
        int dias = 7)
    {
        return await _dashboardRepository
            .ObtenerRespuestasPorDiaAsync(dias);
    }
}