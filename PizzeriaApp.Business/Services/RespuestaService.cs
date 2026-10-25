using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracci�n
// IRespuestaRepository, no de una implementaci�n concreta.
public class RespuestaService : IRespuestaService
{
    private readonly IRespuestaRepository _repository;

    public RespuestaService(IRespuestaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Respuesta>> ObtenerRespuestasAsync()
    {
        return await _repository.ObtenerConRelacionesAsync();
    }

    public async Task<IEnumerable<Respuesta>> BuscarRespuestasAsync(string filtro)
    {
        return await _repository.BuscarAsync(filtro);
    }

    public async Task<Respuesta> GuardarAsync(Respuesta respuesta)
    {
        respuesta.FechaCreacion = DateTime.Now;
        return await _repository.AddAsync(respuesta);
    }

    public async Task ActualizarAsync(Respuesta respuesta)
    {
        await _repository.UpdateAsync(respuesta);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Respuesta?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    // Este m�todo crea una nueva respuesta en el foro, asignando autom�ticamente el estado inicial al crearla.
    // evita que el controlador construya directamente la entidad.
    public async Task<Respuesta> CrearRespuestaForoAsync(
    int temaId,
    int usuarioId,
    string contenido)
    {
        if (temaId <= 0)
        {
            throw new ArgumentException(
                "El tema seleccionado no es v�lido.",
                nameof(temaId));
        }

        if (usuarioId <= 0)
        {
            throw new ArgumentException(
                "El usuario autenticado no es v�lido.",
                nameof(usuarioId));
        }

        if (string.IsNullOrWhiteSpace(contenido))
        {
            throw new ArgumentException(
                "El contenido de la respuesta es obligatorio.",
                nameof(contenido));
        }

        var estadoInicial =
            await _repository.ObtenerEstadoInicialAsync();

        if (estadoInicial is null)
        {
            throw new InvalidOperationException(
                "No existe un estado disponible para registrar la respuesta.");
        }

        var respuesta = new Respuesta
        {
            TemaId = temaId,
            UsuarioId = usuarioId,
            EstadoId = estadoInicial.EstadoId,
            Contenido = contenido.Trim(),
            FechaCreacion = DateTime.Now
        };

        return await _repository.AddAsync(respuesta);
    }
}