using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con los jugadores
public interface IJugadorRepository : IGenericRepository<Jugador>
{
    Task<IEnumerable<Jugador>> ObtenerConEquipoAsync();

    Task<IEnumerable<Jugador>> BuscarAsync(string filtro);
}