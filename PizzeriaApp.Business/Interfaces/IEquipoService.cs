using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle - encapsula �nicamente la l�gica de negocio de Equipo.
public interface IEquipoService
{
    Task<IEnumerable<Equipo>> ObtenerEquiposAsync();

    Task<IEnumerable<Equipo>> BuscarEquiposAsync(string filtro);

    Task<Equipo> GuardarAsync(Equipo equipo);

    Task ActualizarAsync(Equipo equipo);

    Task EliminarAsync(int id);

    Task<Equipo?> ObtenerPorIdAsync(int id);
}