using NetForum.Data.Entities;
using System.Threading.Tasks;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - contrato específico para Usuario.
public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetByCorreoAsync(string correo);

    Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);

    Task<bool> ExisteCorreoAsync(string correo);
}