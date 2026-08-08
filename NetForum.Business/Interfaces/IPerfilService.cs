using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// DP: Service Layer Pattern
// SOLID: Interface Segregation Principle
public interface IPerfilService
{
    Task<Usuario?> ObtenerPerfilAsync(int usuarioId);

    Task<int> ContarTemasAsync(int usuarioId);

    Task<int> ContarRespuestasAsync(int usuarioId);

    Task<int> ContarReaccionesAsync(int usuarioId);
}