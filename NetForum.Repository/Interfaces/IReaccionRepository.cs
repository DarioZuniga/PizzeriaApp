using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle
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

