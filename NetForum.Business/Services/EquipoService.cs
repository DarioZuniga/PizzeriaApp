using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
public class EquipoService : IEquipoService
{
    private readonly IEquipoRepository _repository;

    public EquipoService(IEquipoRepository repository)
    {
        // SOLID: Dependency Inversion Principle
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