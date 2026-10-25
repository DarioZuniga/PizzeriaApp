using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con las reacciones
public interface IReaccionRepository : IGenericRepository<Reaccion> 
{
    Task<IEnumerable<TipoReaccion>> ObtenerTiposAsync();

    Task<Reaccion?> ObtenerAsync(
        int usuarioId,
        int temaId,
        int tipoReaccionId);

    Task<IEnumerable<Reaccion>> ObtenerPorTemaAsync(int temaId);

    Task EliminarAsync(Reaccion reaccion);
}

