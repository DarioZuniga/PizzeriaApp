using NetForum.Data.Entities;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - la interfaz está enfocada en un conjunto específico de operaciones relacionadas con los equipos
public interface IEquipoRepository : IGenericRepository<Equipo>
{
    Task<IEnumerable<Equipo>> ObtenerConLigaAsync();

    Task<IEnumerable<Equipo>> BuscarAsync(string filtro);
}