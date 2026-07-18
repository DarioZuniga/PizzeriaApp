using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface IEtiquetaService
{
    Task<IEnumerable<Etiqueta>> ObtenerEtiquetasAsync();

    Task<IEnumerable<Etiqueta>> BuscarEtiquetasAsync(string filtro);

    Task<Etiqueta> GuardarAsync(Etiqueta etiqueta);

    Task ActualizarAsync(Etiqueta etiqueta);

    Task EliminarAsync(int id);

    Task<Etiqueta?> ObtenerPorIdAsync(int id);
}