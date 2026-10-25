using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Repository.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetByCorreoAsync(string correo);
    Task<Usuario?> GetWithRolAsync(int usuarioId);
    Task<bool> ExisteCorreoAsync(string correo);
}