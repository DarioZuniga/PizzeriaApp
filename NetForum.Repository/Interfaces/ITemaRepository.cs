using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface ITemaRepository : IGenericRepository<Tema>
{
    Task<IEnumerable<Tema>> ObtenerConRelacionesAsync();

    Task<IEnumerable<Tema>> BuscarAsync(string filtro);
}