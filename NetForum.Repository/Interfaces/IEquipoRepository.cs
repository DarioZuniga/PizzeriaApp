using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface IEquipoRepository : IGenericRepository<Equipo>
{
    Task<IEnumerable<Equipo>> ObtenerConLigaAsync();

    Task<IEnumerable<Equipo>> BuscarAsync(string filtro);
}