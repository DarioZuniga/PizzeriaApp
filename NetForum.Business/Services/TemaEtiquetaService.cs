using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
public class TemaEtiquetaService : ITemaEtiquetaService
{
    private readonly ITemaEtiquetaRepository _repository;

    public TemaEtiquetaService(
        ITemaEtiquetaRepository repository)
    {
        // SOLID: Dependency Inversion Principle
        _repository = repository;
    }

    public async Task<IEnumerable<Etiqueta>> ObtenerEtiquetasAsync()
    {
        return await _repository.ObtenerEtiquetasAsync();
    }

    public async Task<IEnumerable<TemaEtiqueta>> ObtenerPorTemaAsync(
        int temaId)
    {
        return await _repository.ObtenerPorTemaAsync(temaId);
    }

    public async Task ReemplazarEtiquetasAsync(
        int temaId,
        IEnumerable<int> etiquetaIds)
    {
        await _repository.ReemplazarEtiquetasAsync(
            temaId,
            etiquetaIds);
    }
}