using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con los reportes
public interface IReporteRepository : IGenericRepository<Reporte>
{
    Task<IEnumerable<Reporte>> ObtenerConRelacionesAsync();

    Task<IEnumerable<Reporte>> BuscarAsync(string filtro);

    Task<IEnumerable<Estado>> ObtenerEstadosAsync();

    Task<Estado?> ObtenerEstadoInicialAsync();
}