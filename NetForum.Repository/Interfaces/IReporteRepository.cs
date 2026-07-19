using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface IReporteRepository : IGenericRepository<Reporte>
{
    Task<IEnumerable<Reporte>> ObtenerConRelacionesAsync();

    Task<IEnumerable<Reporte>> BuscarAsync(string filtro);

    Task<IEnumerable<Estado>> ObtenerEstadosAsync();
}