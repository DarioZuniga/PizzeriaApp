using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con las etiquetas
public interface IEtiquetaRepository : IGenericRepository<Etiqueta>
{
    Task<IEnumerable<Etiqueta>> BuscarAsync(string filtro);
}