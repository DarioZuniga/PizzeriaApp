using NetForum.Data.Entities;

namespace NetForum.Business.Interfaces;

// SOLID: Single Responsibility Principle - encapsula únicamente la lógica de autenticación
// DP: Service Layer Pattern
public interface IAuthService
{
    Task<Usuario?> LoginAsync(string correo, string password); //Será utilizada por el AuthController para autenticar usuarios

    Task<Usuario> RegistrarUsuarioAsync(Usuario usuario, string password); //Permite crear nuevos usuarios con la contraseña encriptada

    Task<bool> ExisteCorreoAsync(string correo); //Evita registrar dos usuarios con el mismo correo electrónico
}