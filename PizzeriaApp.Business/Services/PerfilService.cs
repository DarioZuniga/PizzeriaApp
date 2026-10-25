using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
public class PerfilService : IPerfilService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public PerfilService(IUsuarioRepository usuarioRepository)
    {
        // SOLID: Dependency Inversion Principle
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Usuario?> ObtenerPerfilAsync(int usuarioId)
    {
        return await _usuarioRepository.ObtenerPerfilAsync(usuarioId);
    }

    public async Task<int> ContarTemasAsync(int usuarioId)
    {
        return await _usuarioRepository.ContarTemasAsync(usuarioId);
    }

    public async Task<int> ContarRespuestasAsync(int usuarioId)
    {
        return await _usuarioRepository.ContarRespuestasAsync(usuarioId);
    }

    public async Task<int> ContarReaccionesAsync(int usuarioId)
    {
        return await _usuarioRepository.ContarReaccionesAsync(usuarioId);
    }
}