using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly PizzeriaAppDbContext _context;

    public AuthService(IUsuarioRepository usuarioRepository, PizzeriaAppDbContext context)
    {
        _usuarioRepository = usuarioRepository;
        _context = context;
    }

    public async Task<Usuario?> LoginAsync(string correo, string password)
    {
        var usuario = await _usuarioRepository.GetByCorreoAsync(correo);
        if (usuario is null) return null;

        try
        {
            if (!BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash)) return null;
        }
        catch { return null; }

        return usuario;
    }

    public async Task<Usuario> RegistrarUsuarioAsync(Usuario usuario, string password)
    {
        var existeCorreo = await _usuarioRepository.ExisteCorreoAsync(usuario.Correo);
        if (existeCorreo) throw new InvalidOperationException("Ya existe un usuario registrado con ese correo.");

        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        usuario.FechaRegistro = DateTime.Now;
        return await _usuarioRepository.AddAsync(usuario);
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _usuarioRepository.ExisteCorreoAsync(correo);
    }

    public async Task<Usuario?> ObtenerUsuarioPorCorreoAsync(string correo)
    {
        return await _usuarioRepository.GetByCorreoAsync(correo);
    }

    public async Task<string> GuardarTokenRecuperacionAsync(int usuarioId, string token)
    {
        var tokenRecuperacion = new TokenRecuperacion
        {
            UsuarioId = usuarioId,
            Token = token,
            FechaExpiracion = DateTime.Now.AddHours(1),
            Usado = false
        };

        _context.TokensRecuperacion.Add(tokenRecuperacion);
        await _context.SaveChangesAsync();

        return $"/Account/ResetPassword?token={token}";
    }

    public async Task<bool> CambiarPasswordConTokenAsync(string token, string nuevaPassword)
    {
        var tokenRecuperacion = await _context.TokensRecuperacion
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Token == token);

        if (tokenRecuperacion == null || tokenRecuperacion.Usado || tokenRecuperacion.FechaExpiracion < DateTime.Now)
            return false;

        tokenRecuperacion.Usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);
        tokenRecuperacion.Usado = true;

        await _context.SaveChangesAsync();
        return true;
    }
}