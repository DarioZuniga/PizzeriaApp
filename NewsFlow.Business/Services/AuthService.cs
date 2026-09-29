using NewsFlow.Business.Interfaces;
using NewsFlow.Data.Entities;
using NewsFlow.Repository.Interfaces;

namespace NewsFlow.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracción
// IUsuarioRepository, no de una implementación concreta.
public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthService(IUsuarioRepository usuarioRepository)
    {
        // SOLID: Dependency Inversion Principle
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario?> LoginAsync(string correo, string password)
    {
        var usuario = await _usuarioRepository.GetByCorreoAsync(correo);

        if (usuario is null)
        {
            return null;
        }

        if (usuario.EstadoId != 1)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
        {
            return null;
        }

        bool passwordValido;

        try
        {
            passwordValido = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
        }
        catch
        {
            return null;
        }

        if (!passwordValido)
        {
            return null;
        }

        return usuario;
    }

    public async Task<Usuario> RegistrarUsuarioAsync(Usuario usuario, string password)
    {
        var existeCorreo = await _usuarioRepository.ExisteCorreoAsync(usuario.Correo);

        if (existeCorreo)
        {
            throw new InvalidOperationException("Ya existe un usuario registrado con ese correo.");
        }

        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        usuario.FechaRegistro = DateTime.Now;

        return await _usuarioRepository.AddAsync(usuario);
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _usuarioRepository.ExisteCorreoAsync(correo);
    }
}