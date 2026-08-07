using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con las respuestas
public interface IRespuestaRepository : IGenericRepository<Respuesta>
{
    Task<IEnumerable<Respuesta>> ObtenerConRelacionesAsync();

    Task<IEnumerable<Respuesta>> BuscarAsync(string filtro);

    Task<Estado?> ObtenerEstadoInicialAsync();
}