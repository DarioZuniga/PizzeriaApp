using NewsFlow.Data.Entities;

namespace NewsFlow.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con las etiquetas
public interface IEtiquetaService
{
    Task<IEnumerable<Etiqueta>> ObtenerEtiquetasAsync();

    Task<IEnumerable<Etiqueta>> BuscarEtiquetasAsync(string filtro);

    Task<Etiqueta> GuardarAsync(Etiqueta etiqueta);

    Task ActualizarAsync(Etiqueta etiqueta);

    Task EliminarAsync(int id);

    Task<Etiqueta?> ObtenerPorIdAsync(int id);
}