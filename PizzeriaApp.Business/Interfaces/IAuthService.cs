using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Business.Interfaces;

public interface IAuthService
{
    Task<Usuario?> LoginAsync(string correo, string password);
    Task<Usuario> RegistrarUsuarioAsync(Usuario usuario, string password);
    Task<bool> ExisteCorreoAsync(string correo);

    // Nuevos métodos para recuperación de contraseña
    Task<Usuario?> ObtenerUsuarioPorCorreoAsync(string correo);
    Task GuardarTokenRecuperacionAsync(int usuarioId, string token);
    Task<bool> CambiarPasswordConTokenAsync(string token, string nuevaPassword);
}