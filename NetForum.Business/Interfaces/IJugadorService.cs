using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface IJugadorService
{
    Task<IEnumerable<Jugador>> ObtenerJugadoresAsync();

    Task<IEnumerable<Jugador>> BuscarJugadoresAsync(string filtro);

    Task<Jugador> GuardarAsync(Jugador jugador);

    Task ActualizarAsync(Jugador jugador);

    Task EliminarAsync(int id);

    Task<Jugador?> ObtenerPorIdAsync(int id);
}