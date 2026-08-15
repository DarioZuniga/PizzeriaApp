using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
public interface ITemaEtiquetaRepository
{
    Task<IEnumerable<Etiqueta>> ObtenerEtiquetasAsync();

    Task<IEnumerable<TemaEtiqueta>> ObtenerPorTemaAsync(int temaId);

    Task ReemplazarEtiquetasAsync(
        int temaId,
        IEnumerable<int> etiquetaIds);
}