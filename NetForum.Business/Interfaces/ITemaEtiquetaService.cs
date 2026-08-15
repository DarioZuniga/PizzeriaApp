using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface ITemaEtiquetaService
{
    Task<IEnumerable<Etiqueta>> ObtenerEtiquetasAsync();
    Task<IEnumerable<TemaEtiqueta>> ObtenerPorTemaAsync(int temaId);
    Task ReemplazarEtiquetasAsync(
        int temaId,
        IEnumerable<int> etiquetaIds);
}
