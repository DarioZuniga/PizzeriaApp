using NewsFlow.Data.Entities;

namespace NewsFlow.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle - aplica principalmente: encapsula la
// lógica de negocio de Tema. Exceptuando la persistencia de datos, que se delega a los repositorios.
public interface ITemaService
{
    // Métodos existentes del CRUD administrativo
    Task<IEnumerable<Tema>> ObtenerTemasAsync();

    Task<IEnumerable<Tema>> BuscarTemasAsync(string filtro);

    Task<Tema> GuardarAsync(Tema tema);

    Task ActualizarAsync(Tema tema);

    Task EliminarAsync(int id);

    Task<Tema?> ObtenerPorIdAsync(int id);
    // Método debúsqueda de feed con filtros opcionales
    Task<IEnumerable<Tema>> ObtenerFeedAsync(
     string? buscar,
     int? categoriaId,
     int? etiquetaId);

    Task<Tema?> ObtenerDetalleAsync(int temaId);

    Task<IEnumerable<Categoria>> ObtenerCategoriasAsync();

    Task<Tema> CrearTemaForoAsync(
    int usuarioId,
    int categoriaId,
    string titulo,
    string contenido);

    Task ActualizarTemaForoAsync(
    int temaId,
    int usuarioId,
    int categoriaId,
    string titulo,
    string contenido);
}