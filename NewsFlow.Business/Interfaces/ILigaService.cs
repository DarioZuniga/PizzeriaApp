using NewsFlow.Data.Entities;

namespace NewsFlow.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con las ligas
public interface ILigaService
{
    Task<IEnumerable<Liga>> ObtenerLigasAsync();

    Task<IEnumerable<Liga>> BuscarLigasAsync(string filtro);

    Task<Liga> GuardarAsync(Liga liga);

    Task ActualizarAsync(Liga liga);

    Task EliminarAsync(int id);

    Task<Liga?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteAsync(int id);
}