using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con las respuestas
public interface IRespuestaService
{
    Task<IEnumerable<Respuesta>> ObtenerRespuestasAsync();

    Task<IEnumerable<Respuesta>> BuscarRespuestasAsync(string filtro);

    Task<Respuesta> GuardarAsync(Respuesta respuesta);

    Task ActualizarAsync(Respuesta respuesta);

    Task EliminarAsync(int id);

    Task<Respuesta?> ObtenerPorIdAsync(int id);

    Task<Respuesta> CrearRespuestaForoAsync(
    int temaId,
    int usuarioId,
    string contenido);
}