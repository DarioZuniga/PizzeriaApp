using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con las etiquetas
public interface IEtiquetaService
{
    Task<IEnumerable<Etiqueta>> ObtenerEtiquetasAsync();

    Task<IEnumerable<Etiqueta>> BuscarEtiquetasAsync(string filtro);

    Task<Etiqueta> GuardarAsync(Etiqueta etiqueta);

    Task ActualizarAsync(Etiqueta etiqueta);

    Task EliminarAsync(int id);

    Task<Etiqueta?> ObtenerPorIdAsync(int id);
}