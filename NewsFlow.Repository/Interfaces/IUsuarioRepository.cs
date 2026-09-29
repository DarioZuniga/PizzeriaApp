using NewsFlow.Data.Entities;
using System.Threading.Tasks;

namespace NewsFlow.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - contrato específico para Usuario.
public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetByCorreoAsync(string correo);

    Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);

    Task<bool> ExisteCorreoAsync(string correo);

    // consultas para el perfil del usuario
    Task<Usuario?> ObtenerPerfilAsync(int usuarioId);
    
    Task<int> ContarTemasAsync(int usuarioId);

    Task<int> ContarRespuestasAsync(int usuarioId);

    Task<int> ContarReaccionesAsync(int usuarioId);
}