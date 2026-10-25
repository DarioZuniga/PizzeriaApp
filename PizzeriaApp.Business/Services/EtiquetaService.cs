using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracci�n
// IEtiquetaRepository, no de una implementaci�n concreta.
public class EtiquetaService : IEtiquetaService
{
    private readonly IEtiquetaRepository _repository;

    public EtiquetaService(IEtiquetaRepository repository)
    {
        // SOLID: Dependency Inversion Principle
        _repository = repository;
    }

    public async Task<IEnumerable<Etiqueta>> ObtenerEtiquetasAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<Etiqueta>> BuscarEtiquetasAsync(string filtro)
    {
        return await _repository.BuscarAsync(filtro);
    }

    public async Task<Etiqueta> GuardarAsync(Etiqueta etiqueta)
    {
        return await _repository.AddAsync(etiqueta);
    }

    public async Task ActualizarAsync(Etiqueta etiqueta)
    {
        await _repository.UpdateAsync(etiqueta);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Etiqueta?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}