using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Repository.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(PizzeriaAppDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Estado)
            .FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task<Usuario?> GetWithRolAsync(int usuarioId)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Estado)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _context.Usuarios.AnyAsync(u => u.Correo == correo);
    }
}