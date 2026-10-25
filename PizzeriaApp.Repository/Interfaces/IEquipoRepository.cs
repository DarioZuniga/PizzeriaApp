using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz est� enfocada en un conjunto espec�fico de operaciones relacionadas con los equipos
public interface IEquipoRepository : IGenericRepository<Equipo>
{
    Task<IEnumerable<Equipo>> ObtenerConLigaAsync();

    Task<IEnumerable<Equipo>> BuscarAsync(string filtro);
}