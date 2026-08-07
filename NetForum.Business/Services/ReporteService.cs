using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;
// DP: Service Layer Pattern
//SOLID: Single Responsibility Principle - la clase tiene la responsabilidad de manejar la lógica de negocio relacionada con los reportes
public class ReporteService : IReporteService
{
    // SOLID: Dependency Inversion Principle - la clase depende de la abstracción
    // IReporteRepository, no de una implementación concreta.
    private readonly IReporteRepository _repository;

    public ReporteService(IReporteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Reporte>> ObtenerReportesAsync()
    {
        return await _repository.ObtenerConRelacionesAsync();
    }

    public async Task<IEnumerable<Reporte>> BuscarReportesAsync(string filtro)
    {
        return await _repository.BuscarAsync(filtro);
    }

    public async Task<IEnumerable<Estado>> ObtenerEstadosAsync()
    {
        return await _repository.ObtenerEstadosAsync();
    }

    public async Task<Reporte> GuardarAsync(Reporte reporte)
    {
        reporte.FechaReporte = DateTime.Now;

        return await _repository.AddAsync(reporte);
    }

    public async Task ActualizarAsync(Reporte reporte)
    {
        await _repository.UpdateAsync(reporte);
    }

    public async Task CambiarEstadoAsync(int reporteId, int estadoId)
    {
        var reporte = await _repository.GetByIdAsync(reporteId);

        if (reporte is null)
        {
            throw new KeyNotFoundException("El reporte no existe.");
        }

        var estados = await _repository.ObtenerEstadosAsync();

        if (!estados.Any(e => e.EstadoId == estadoId))
        {
            throw new InvalidOperationException(
                "El estado seleccionado no existe.");
        }

        reporte.EstadoId = estadoId;

        await _repository.UpdateAsync(reporte);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Reporte?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Reporte> CrearReporteForoAsync(
    int temaId,
    int usuarioId,
    string motivo)
    {
        if (temaId <= 0)
        {
            throw new ArgumentException(
                "El tema seleccionado no es válido.",
                nameof(temaId));
        }

        if (usuarioId <= 0)
        {
            throw new ArgumentException(
                "El usuario autenticado no es válido.",
                nameof(usuarioId));
        }

        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new ArgumentException(
                "El motivo del reporte es obligatorio.",
                nameof(motivo));
        }

        var estadoInicial =
            await _repository.ObtenerEstadoInicialAsync();

        if (estadoInicial is null)
        {
            throw new InvalidOperationException(
                "No existe un estado disponible para registrar el reporte.");
        }

        var reporte = new Reporte
        {
            TemaId = temaId,
            UsuarioId = usuarioId,
            EstadoId = estadoInicial.EstadoId,
            Motivo = motivo.Trim(),
            FechaReporte = DateTime.Now
        };

        return await _repository.AddAsync(reporte);
    }
}