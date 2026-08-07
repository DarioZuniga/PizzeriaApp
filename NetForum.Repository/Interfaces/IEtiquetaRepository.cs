using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con las etiquetas
public interface IEtiquetaRepository : IGenericRepository<Etiqueta>
{
    Task<IEnumerable<Etiqueta>> BuscarAsync(string filtro);
}