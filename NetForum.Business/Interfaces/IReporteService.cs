using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface IReporteService
{
    Task<IEnumerable<Reporte>> ObtenerReportesAsync();

    Task<IEnumerable<Reporte>> BuscarReportesAsync(string filtro);

    Task<IEnumerable<Estado>> ObtenerEstadosAsync();

    Task<Reporte> GuardarAsync(Reporte reporte);

    Task ActualizarAsync(Reporte reporte);

    Task CambiarEstadoAsync(int reporteId, int estadoId);

    Task EliminarAsync(int id);

    Task<Reporte?> ObtenerPorIdAsync(int id);

    Task<Reporte> CrearReporteForoAsync(
    int temaId,
    int usuarioId,
    string motivo);
}

