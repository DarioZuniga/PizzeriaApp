using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface ILigaRepository : IGenericRepository<Liga>
{
    Task<IEnumerable<Liga>> BuscarPorNombreOPaisAsync(string filtro);
}