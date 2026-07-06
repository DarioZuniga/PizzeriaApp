using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle
public class TemaRepository : GenericRepository<Tema>, ITemaRepository
{
    public TemaRepository(NetForumDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Tema>> ObtenerConRelacionesAsync()
    {
        return await _context.Temas
            .Include(t => t.Usuario)
            .Include(t => t.Categoria)
            .Include(t => t.Estado)
            .OrderByDescending(t => t.FechaCreacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tema>> BuscarAsync(string filtro)
    {
        return await _context.Temas
            .Include(t => t.Usuario)
            .Include(t => t.Categoria)
            .Include(t => t.Estado)
            .Where(t =>
                t.Titulo.Contains(filtro) ||
                t.Contenido.Contains(filtro) ||
                t.Categoria.Nombre.Contains(filtro) ||
                t.Usuario.NombreUsuario.Contains(filtro))
            .OrderByDescending(t => t.FechaCreacion)
            .ToListAsync();
    }
}
