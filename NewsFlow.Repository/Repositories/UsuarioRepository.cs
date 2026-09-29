using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NewsFlow.Data;
using NewsFlow.Data.Entities;
using NewsFlow.Repository.Interfaces;

namespace NewsFlow.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - esta clase solo maneja consultas específicas de Usuario.
public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(NewsFlowDbContext context)
        : base(context)
    {
    }

    public async Task<Usuario?> GetByCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Estado)
            .FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Estado)
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Correo == correo);
    }
    // DP: Repository Pattern
    //OOBTIENE EL PERFIL DEL USUARIO CON SUS TEMAS, RESPUESTAS Y REACCIONES
    // SOLID: Single Responsibility Principle - esta clase solo maneja consultas específicas de Usuario.
    public async Task<Usuario?> ObtenerPerfilAsync(int usuarioId)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .Include(u => u.Estado)
            .Include(u => u.Temas)
                .ThenInclude(t => t.Categoria)
            .Include(u => u.Temas)
                .ThenInclude(t => t.Estado)
            .Include(u => u.Temas)
                .ThenInclude(t => t.Respuestas)
            .Include(u => u.Temas)
                .ThenInclude(t => t.Reacciones)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
    }

    public async Task<int> ContarTemasAsync(int usuarioId)
    {
        return await _context.Temas
            .CountAsync(t => t.UsuarioId == usuarioId);
    }

    public async Task<int> ContarRespuestasAsync(int usuarioId)
    {
        return await _context.Respuestas
            .CountAsync(r => r.UsuarioId == usuarioId);
    }

    public async Task<int> ContarReaccionesAsync(int usuarioId)
    {
        return await _context.Reacciones
            .CountAsync(r => r.UsuarioId == usuarioId);
    }
}