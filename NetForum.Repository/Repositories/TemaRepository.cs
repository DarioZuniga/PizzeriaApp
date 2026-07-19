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

    public async Task<IEnumerable<Tema>> ObtenerFeedAsync(
    string? buscar,
    int? categoriaId)
    {
        var query = _context.Temas
            .AsNoTracking()
            .Include(t => t.Usuario)
            .Include(t => t.Categoria)
            .Include(t => t.Estado)
            .Include(t => t.Respuestas)
            .Include(t => t.Reacciones)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var filtro = buscar.Trim();

            query = query.Where(t =>
                t.Titulo.Contains(filtro) ||
                t.Contenido.Contains(filtro));
        }

        if (categoriaId.HasValue && categoriaId.Value > 0)
        {
            query = query.Where(t =>
                t.CategoriaId == categoriaId.Value);
        }

        return await query
            .OrderByDescending(t => t.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Tema?> ObtenerDetalleAsync(int temaId)
    {
        return await _context.Temas
            .AsNoTracking()
            .Include(t => t.Usuario)
            .Include(t => t.Categoria)
            .Include(t => t.Estado)
            .Include(t => t.Respuestas)
                .ThenInclude(r => r.Usuario)
            .Include(t => t.Respuestas)
                .ThenInclude(r => r.Estado)
            .Include(t => t.Reacciones)
            .FirstOrDefaultAsync(t => t.TemaId == temaId);
    }

    public async Task<IEnumerable<Categoria>> ObtenerCategoriasAsync()
    {
        return await _context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }
}
