using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracci�n
// ICategoriaRepository, no de una implementaci�n concreta.
public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        // SOLID: Dependency Inversion Principle
        _repository = repository;
    }

    public async Task<IEnumerable<Categoria>> ObtenerCategoriasAsync()
    {
        return await _repository.ObtenerCategoriasActivasAsync();
    }

    public async Task<IEnumerable<Categoria>> BuscarCategoriasAsync(string nombre)
    {
        return await _repository.BuscarPorNombreAsync(nombre);
    }

    public async Task<Categoria> GuardarAsync(Categoria categoria)
    {
        return await _repository.AddAsync(categoria);
    }

    public async Task ActualizarAsync(Categoria categoria)
    {
        await _repository.UpdateAsync(categoria);
    }

    public async Task EliminarAsync(int id)
    {
        var categoria = await _repository.GetByIdAsync(id);

        if (categoria is null)
        {
            throw new KeyNotFoundException(
                "La categor�a que intenta eliminar no existe.");
        }

        var tieneTemas =
            await _repository.TieneTemasAsociadosAsync(id);

        if (tieneTemas)
        {
            throw new InvalidOperationException(
                "No se puede eliminar esta categor�a porque tiene temas asociados.");
        }

        await _repository.DeleteAsync(id);
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }
}