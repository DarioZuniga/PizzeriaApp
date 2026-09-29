using NewsFlow.Data.Entities;

namespace NewsFlow.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con los jugadores
public interface IJugadorRepository : IGenericRepository<Jugador>
{
    Task<IEnumerable<Jugador>> ObtenerConEquipoAsync();

    Task<IEnumerable<Jugador>> BuscarAsync(string filtro);
}