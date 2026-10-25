using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracci�n
// IJugadorRepository, no de una implementaci�n concreta.
public class JugadorService : IJugadorService
{
    private readonly IJugadorRepository _repository;

    public JugadorService(IJugadorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Jugador>> ObtenerJugadoresAsync()
    {
        return await _repository.ObtenerConEquipoAsync();
    }

    public async Task<IEnumerable<Jugador>> BuscarJugadoresAsync(string filtro)
    {
        return await _repository.BuscarAsync(filtro);
    }

    public async Task<Jugador> GuardarAsync(Jugador jugador)
    {
        return await _repository.AddAsync(jugador);
    }

    public async Task ActualizarAsync(Jugador jugador)
    {
        await _repository.UpdateAsync(jugador);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Jugador?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}