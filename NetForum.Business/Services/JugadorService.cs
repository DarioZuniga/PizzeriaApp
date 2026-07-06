using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
public class JugadorService : IJugadorService
{
    private readonly IJugadorRepository _repository;

    public JugadorService(IJugadorRepository repository)
    {
        // SOLID: Dependency Inversion Principle
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