using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con las respuestas
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