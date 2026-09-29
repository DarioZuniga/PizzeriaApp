using NewsFlow.Data.Entities;

namespace NewsFlow.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - se aplica parcialmente a la interfaz ITemaRepository, ya que
// combina métodos relacionados con el CRUD administrativo y métodos específicos del FrontOffice.
public interface ITemaRepository : IGenericRepository<Tema>
{
    // Métodos existentes del CRUD administrativo.
    Task<IEnumerable<Tema>> ObtenerConRelacionesAsync(); // task que devuelve todos los temas con sus relaciones
                                                         // (usuario, categoría, estado, respuestas, reacciones, reportes y etiquetas).

    Task<IEnumerable<Tema>> BuscarAsync(string filtro); // task que devuelve los temas que coinciden con un filtro
                                                        // de búsqueda en el título o contenido.

    // Métodos del FrontOffice.
    Task<IEnumerable<Tema>> ObtenerFeedAsync( // task que devuelve los temas para el feed del foro,
                                              // con soporte para paginación y filtrado por categoría o etiqueta.
      string? buscar,
      int? categoriaId,
      int? etiquetaId);

    Task<Tema?> ObtenerDetalleAsync(int temaId); // task que devuelve el detalle de un tema específico, incluyendo sus relaciones
                                                 // (usuario, categoría, estado, respuestas, reacciones, reportes y etiquetas).

    Task<IEnumerable<Categoria>> ObtenerCategoriasAsync(); // task que devuelve todas las categorías disponibles en el foro.

    Task<Estado?> ObtenerEstadoInicialAsync(); // task que devuelve el estado inicial para un nuevo tema,
                                               // que puede ser "Abierto" o "Pendiente de revisión".
}