using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracci�n
// ITemaRepository, no de una implementaci�n concreta.
public class TemaService : ITemaService
{
    private readonly ITemaRepository _repository;

    public TemaService(ITemaRepository repository)
    {
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
    //M�todo de b�squeda de feed con filtros opcionales
    public async Task<IEnumerable<Tema>> ObtenerFeedAsync(
    string? buscar,
    int? categoriaId,
    int? etiquetaId)
    {
        return await _repository.ObtenerFeedAsync(
            buscar,
            categoriaId,
            etiquetaId);
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
                "El usuario autenticado no es v�lido.",
                nameof(usuarioId));
        }

        if (categoriaId <= 0)
        {
            throw new ArgumentException(
                "La categor�a seleccionada no es v�lida.",
                nameof(categoriaId));
        }

        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException(
                "El t�tulo es obligatorio.",
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
                "La categor�a seleccionada no existe.");
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
    public async Task ActualizarTemaForoAsync( //m�todo para actualizar un tema del foro, con validaciones
                                               //de permisos y datos
    int temaId,
    int usuarioId,
    int categoriaId,
    string titulo,
    string contenido)
    {
        var tema = await _repository.GetByIdAsync(temaId);

        if (tema is null)
        {
            throw new KeyNotFoundException(
                "El tema no existe.");
        }

        if (tema.UsuarioId != usuarioId)
        {
            throw new UnauthorizedAccessException(  //Seguridad: Validaci�n de permisos para modificar un tema 
                "No tiene permisos para modificar este tema.");
        }

        if (categoriaId <= 0)
        {
            throw new ArgumentException(
                "La categor�a seleccionada no es v�lida.");
        }

        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException(
                "El t�tulo es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(contenido))
        {
            throw new ArgumentException(
                "El contenido es obligatorio.");
        }

        tema.CategoriaId = categoriaId;
        tema.Titulo = titulo.Trim();
        tema.Contenido = contenido.Trim();

        await _repository.UpdateAsync(tema);
    }
}