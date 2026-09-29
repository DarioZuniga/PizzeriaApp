using NewsFlow.Data.Entities;

namespace NewsFlow.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con los reportes
public interface IReporteRepository : IGenericRepository<Reporte>
{
    Task<IEnumerable<Reporte>> ObtenerConRelacionesAsync();

    Task<IEnumerable<Reporte>> BuscarAsync(string filtro);

    Task<IEnumerable<Estado>> ObtenerEstadosAsync();

    Task<Estado?> ObtenerEstadoInicialAsync();
}