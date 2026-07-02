using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - solo declara operaciones específicas de Categoría.
public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<IEnumerable<Categoria>> ObtenerCategoriasActivasAsync();

    Task<IEnumerable<Categoria>> BuscarPorNombreAsync(string nombre);
}