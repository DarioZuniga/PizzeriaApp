using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - se aplica parcialmente a la interfaz ITemaRepository, ya que
// combina métodos relacionados con el CRUD administrativo y métodos específicos del FrontOffice.
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

    Task<Estado?> ObtenerEstadoInicialAsync();
}