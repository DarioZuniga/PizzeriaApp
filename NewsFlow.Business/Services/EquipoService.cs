using NewsFlow.Business.Interfaces;
using NewsFlow.Data.Entities;
using NewsFlow.Repository.Interfaces;

namespace NewsFlow.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracción
// IEquipoRepository, no de una implementación concreta.
public class EquipoService : IEquipoService
{
    private readonly IEquipoRepository _repository;

    public EquipoService(IEquipoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Equipo>> ObtenerEquiposAsync()
    {
        return await _repository.ObtenerConLigaAsync();
    }

    public async Task<IEnumerable<Equipo>> BuscarEquiposAsync(string filtro)
    {
        return await _repository.BuscarAsync(filtro);
    }

    public async Task<Equipo> GuardarAsync(Equipo equipo)
    {
        return await _repository.AddAsync(equipo);
    }

    public async Task ActualizarAsync(Equipo equipo)
    {
        await _repository.UpdateAsync(equipo);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Equipo?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}