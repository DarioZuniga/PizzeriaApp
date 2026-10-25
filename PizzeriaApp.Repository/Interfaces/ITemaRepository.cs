using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - se aplica parcialmente a la interfaz ITemaRepository, ya que
// combina m�todos relacionados con el CRUD administrativo y m�todos espec�ficos del FrontOffice.
public interface ITemaRepository : IGenericRepository<Tema>
{
    // M�todos existentes del CRUD administrativo.
    Task<IEnumerable<Tema>> ObtenerConRelacionesAsync(); // task que devuelve todos los temas con sus relaciones
                                                         // (usuario, categor�a, estado, respuestas, reacciones, reportes y etiquetas).

    Task<IEnumerable<Tema>> BuscarAsync(string filtro); // task que devuelve los temas que coinciden con un filtro
                                                        // de b�squeda en el t�tulo o contenido.

    // M�todos del FrontOffice.
    Task<IEnumerable<Tema>> ObtenerFeedAsync( // task que devuelve los temas para el feed del foro,
                                              // con soporte para paginaci�n y filtrado por categor�a o etiqueta.
      string? buscar,
      int? categoriaId,
      int? etiquetaId);

    Task<Tema?> ObtenerDetalleAsync(int temaId); // task que devuelve el detalle de un tema espec�fico, incluyendo sus relaciones
                                                 // (usuario, categor�a, estado, respuestas, reacciones, reportes y etiquetas).

    Task<IEnumerable<Categoria>> ObtenerCategoriasAsync(); // task que devuelve todas las categor�as disponibles en el foro.

    Task<Estado?> ObtenerEstadoInicialAsync(); // task que devuelve el estado inicial para un nuevo tema,
                                               // que puede ser "Abierto" o "Pendiente de revisi�n".
}