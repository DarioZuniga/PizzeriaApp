using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> ObtenerCategoriasAsync();

    Task<IEnumerable<Categoria>> BuscarCategoriasAsync(string nombre);

    Task<Categoria> GuardarAsync(Categoria categoria);

    Task ActualizarAsync(Categoria categoria);

    Task EliminarAsync(int id);

    Task<Categoria?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteAsync(int id);
}