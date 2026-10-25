using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - solo declara operaciones espec�ficas de Categor�a.
public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    Task<IEnumerable<Categoria>> ObtenerCategoriasActivasAsync();

    Task<IEnumerable<Categoria>> BuscarPorNombreAsync(string nombre);

    Task<bool> TieneTemasAsociadosAsync(int categoriaId); // Su funcionalidad es verificar si
                                                          // una categor�a tiene temas asociados antes de eliminarla.
}