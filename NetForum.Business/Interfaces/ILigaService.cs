using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
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