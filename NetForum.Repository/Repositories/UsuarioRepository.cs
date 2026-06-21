using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - esta clase solo maneja consultas específicas de Usuario.
public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(NetForumDbContext context)
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
}