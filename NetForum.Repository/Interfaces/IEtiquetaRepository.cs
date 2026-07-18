using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface IEtiquetaRepository : IGenericRepository<Etiqueta>
{
    Task<IEnumerable<Etiqueta>> BuscarAsync(string filtro);
}