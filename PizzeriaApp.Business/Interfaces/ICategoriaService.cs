using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con las categor�as
public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> ObtenerCategoriasAsync();

    Task<IEnumerable<Categoria>> BuscarCategoriasAsync(string nombre);

    Task<Categoria> GuardarAsync(Categoria categoria);

    Task ActualizarAsync(Categoria categoria);

    Task EliminarAsync(int id);

    Task<Categoria?> ObtenerPorIdAsync(int id);

    Task<bool> ExisteAsync(int id);
}