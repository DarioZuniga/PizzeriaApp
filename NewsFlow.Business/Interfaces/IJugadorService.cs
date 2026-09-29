using NewsFlow.Data.Entities;

namespace NewsFlow.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con los jugadores
public interface IJugadorService
{
    Task<IEnumerable<Jugador>> ObtenerJugadoresAsync();

    Task<IEnumerable<Jugador>> BuscarJugadoresAsync(string filtro);

    Task<Jugador> GuardarAsync(Jugador jugador);

    Task ActualizarAsync(Jugador jugador);

    Task EliminarAsync(int id);

    Task<Jugador?> ObtenerPorIdAsync(int id);
}