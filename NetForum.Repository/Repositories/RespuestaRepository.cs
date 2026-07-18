using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle
public class RespuestaRepository : GenericRepository<Respuesta>, IRespuestaRepository
{
    public RespuestaRepository(NetForumDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Respuesta>> ObtenerConRelacionesAsync()
    {
        return await _context.Respuestas
            .Include(r => r.Tema)
            .Include(r => r.Usuario)
            .Include(r => r.Estado)
            .OrderByDescending(r => r.FechaCreacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<Respuesta>> BuscarAsync(string filtro)
    {
        return await _context.Respuestas
            .Include(r => r.Tema)
            .Include(r => r.Usuario)
            .Include(r => r.Estado)
            .Where(r =>
                r.Contenido.Contains(filtro) ||
                r.Tema.Titulo.Contains(filtro) ||
                r.Usuario.NombreUsuario.Contains(filtro))
            .OrderByDescending(r => r.FechaCreacion)
            .ToListAsync();
    }
}