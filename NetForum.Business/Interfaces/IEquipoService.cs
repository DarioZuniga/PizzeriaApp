using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface IEquipoService
{
    Task<IEnumerable<Equipo>> ObtenerEquiposAsync();

    Task<IEnumerable<Equipo>> BuscarEquiposAsync(string filtro);

    Task<Equipo> GuardarAsync(Equipo equipo);

    Task ActualizarAsync(Equipo equipo);

    Task EliminarAsync(int id);

    Task<Equipo?> ObtenerPorIdAsync(int id);
}