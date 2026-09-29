using NewsFlow.Business.Models;
using NewsFlow.Data.Entities;

namespace NewsFlow.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con las reacciones
public interface IReaccionService
{
    Task<IEnumerable<TipoReaccion>> ObtenerTiposAsync();

    Task<IEnumerable<Reaccion>> ObtenerPorTemaAsync(int temaId);

    Task<ResultadoReaccion> AlternarReaccionAsync(
        int usuarioId,
        int temaId,
        int tipoReaccionId);
}