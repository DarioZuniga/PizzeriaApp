using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
public class TemaService : ITemaService
{
    private readonly ITemaRepository _repository;

    public TemaService(ITemaRepository repository)
    {
        // SOLID: Dependency Inversion Principle
        _repository = repository;
    }

    public async Task<IEnumerable<Tema>> ObtenerTemasAsync()
    {
        return await _repository.ObtenerConRelacionesAsync();
    }

    public async Task<IEnumerable<Tema>> BuscarTemasAsync(string filtro)
    {
        return await _repository.BuscarAsync(filtro);
    }

    public async Task<Tema> GuardarAsync(Tema tema)
    {
        tema.FechaCreacion = DateTime.Now;
        return await _repository.AddAsync(tema);
    }

    public async Task ActualizarAsync(Tema tema)
    {
        await _repository.UpdateAsync(tema);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Tema?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    //Método de búsqueda de feed con filtros opcionales
    public async Task<IEnumerable<Tema>> ObtenerFeedAsync(
    string? buscar,
    int? categoriaId)
    {
        return await _repository.ObtenerFeedAsync(
            buscar,
            categoriaId);
    }

    public async Task<Tema?> ObtenerDetalleAsync(int temaId)
    {
        return await _repository.ObtenerDetalleAsync(temaId);
    }

    public async Task<IEnumerable<Categoria>> ObtenerCategoriasAsync()
    {
        return await _repository.ObtenerCategoriasAsync();
    }

    public async Task<Tema> CrearTemaForoAsync(
    int usuarioId,
    int categoriaId,
    string titulo,
    string contenido)
    {
        if (usuarioId <= 0)
        {
            throw new ArgumentException(
                "El usuario autenticado no es válido.",
                nameof(usuarioId));
        }

        if (categoriaId <= 0)
        {
            throw new ArgumentException(
                "La categoría seleccionada no es válida.",
                nameof(categoriaId));
        }

        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException(
                "El título es obligatorio.",
                nameof(titulo));
        }

        if (string.IsNullOrWhiteSpace(contenido))
        {
            throw new ArgumentException(
                "El contenido es obligatorio.",
                nameof(contenido));
        }

        var categorias = await _repository.ObtenerCategoriasAsync();

        if (!categorias.Any(c => c.CategoriaId == categoriaId))
        {
            throw new InvalidOperationException(
                "La categoría seleccionada no existe.");
        }

        var estadoInicial =
            await _repository.ObtenerEstadoInicialAsync();

        if (estadoInicial is null)
        {
            throw new InvalidOperationException(
                "No existe un estado disponible para publicar el tema.");
        }

        var tema = new Tema
        {
            UsuarioId = usuarioId,
            CategoriaId = categoriaId,
            EstadoId = estadoInicial.EstadoId,
            Titulo = titulo.Trim(),
            Contenido = contenido.Trim(),
            FechaCreacion = DateTime.Now
        };

        return await _repository.AddAsync(tema);
    }
}