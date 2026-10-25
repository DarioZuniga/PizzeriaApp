using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con los jugadores
public interface IJugadorService
{
    Task<IEnumerable<Jugador>> ObtenerJugadoresAsync();

    Task<IEnumerable<Jugador>> BuscarJugadoresAsync(string filtro);

    Task<Jugador> GuardarAsync(Jugador jugador);

    Task ActualizarAsync(Jugador jugador);

    Task EliminarAsync(int id);

    Task<Jugador?> ObtenerPorIdAsync(int id);
}