using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con las respuestas
public interface IRespuestaRepository : IGenericRepository<Respuesta>
{
    Task<IEnumerable<Respuesta>> ObtenerConRelacionesAsync();

    Task<IEnumerable<Respuesta>> BuscarAsync(string filtro);

    Task<Estado?> ObtenerEstadoInicialAsync();
}