using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface ITemaService
{
    Task<IEnumerable<Tema>> ObtenerTemasAsync();

    Task<IEnumerable<Tema>> BuscarTemasAsync(string filtro);

    Task<Tema> GuardarAsync(Tema tema);

    Task ActualizarAsync(Tema tema);

    Task EliminarAsync(int id);

    Task<Tema?> ObtenerPorIdAsync(int id);
}