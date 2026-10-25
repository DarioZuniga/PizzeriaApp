using PizzeriaApp.Business.Models;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con las reacciones
public interface IReaccionService
{
    Task<IEnumerable<TipoReaccion>> ObtenerTiposAsync();

    Task<IEnumerable<Reaccion>> ObtenerPorTemaAsync(int temaId);

    Task<ResultadoReaccion> AlternarReaccionAsync(
        int usuarioId,
        int temaId,
        int tipoReaccionId);
}