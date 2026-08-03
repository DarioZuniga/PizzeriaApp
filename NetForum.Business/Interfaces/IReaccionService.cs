using NetForum.Business.Models;
using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface IReaccionService
{
    Task<IEnumerable<TipoReaccion>> ObtenerTiposAsync();

    Task<IEnumerable<Reaccion>> ObtenerPorTemaAsync(int temaId);

    Task<ResultadoReaccion> AlternarReaccionAsync(
        int usuarioId,
        int temaId,
        int tipoReaccionId);
}