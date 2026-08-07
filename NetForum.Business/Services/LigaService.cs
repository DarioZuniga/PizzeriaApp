using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle - la clase se encarga únicamente de la
// lógica de negocio relacionada con las ligas.
public class LigaService : ILigaService
{
    private readonly ILigaRepository _repository;

    public LigaService(ILigaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Liga>> ObtenerLigasAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<Liga>> BuscarLigasAsync(string filtro)
    {
        return await _repository.BuscarPorNombreOPaisAsync(filtro);
    }

    public async Task<Liga> GuardarAsync(Liga liga)
    {
        return await _repository.AddAsync(liga);
    }

    public async Task ActualizarAsync(Liga liga)
    {
        await _repository.UpdateAsync(liga);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Liga?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }
}