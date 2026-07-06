using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface IJugadorRepository : IGenericRepository<Jugador>
{
    Task<IEnumerable<Jugador>> ObtenerConEquipoAsync();

    Task<IEnumerable<Jugador>> BuscarAsync(string filtro);
}