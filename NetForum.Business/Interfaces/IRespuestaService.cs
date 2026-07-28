using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
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