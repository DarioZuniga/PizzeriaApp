using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

// SOLID: Single Responsibility Principle - encapsula �nicamente la l�gica de autenticaci�n
// DP: Service Layer Pattern
public interface IAuthService
{
    Task<Usuario?> LoginAsync(string correo, string password); //Ser� utilizada por el AuthController para autenticar usuarios

    Task<Usuario> RegistrarUsuarioAsync(Usuario usuario, string password); //Permite crear nuevos usuarios con la contrase�a encriptada

    Task<bool> ExisteCorreoAsync(string correo); //Evita registrar dos usuarios con el mismo correo electr�nico
}