using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface ITemaRepository : IGenericRepository<Tema>
{
    // Métodos existentes del CRUD administrativo.
    Task<IEnumerable<Tema>> ObtenerConRelacionesAsync();

    Task<IEnumerable<Tema>> BuscarAsync(string filtro);

    // Métodos del FrontOffice.
    Task<IEnumerable<Tema>> ObtenerFeedAsync(
        string? buscar,
        int? categoriaId);

    Task<Tema?> ObtenerDetalleAsync(int temaId);

    Task<IEnumerable<Categoria>> ObtenerCategoriasAsync();
}