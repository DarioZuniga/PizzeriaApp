using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
public class TemaService : ITemaService
{
    private readonly ITemaRepository _repository;

    public TemaService(ITemaRepository repository)
    {
        // SOLID: Dependency Inversion Principle
        _repository = repository;
    }

    public async Task<IEnumerable<Tema>> ObtenerTemasAsync()
    {
        return await _repository.ObtenerConRelacionesAsync();
    }

    public async Task<IEnumerable<Tema>> BuscarTemasAsync(string filtro)
    {
        return await _repository.BuscarAsync(filtro);
    }

    public async Task<Tema> GuardarAsync(Tema tema)
    {
        tema.FechaCreacion = DateTime.Now;
        return await _repository.AddAsync(tema);
    }

    public async Task ActualizarAsync(Tema tema)
    {
        await _repository.UpdateAsync(tema);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Tema?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}