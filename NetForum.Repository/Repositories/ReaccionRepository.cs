using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - la clase tiene una única responsabilidad: manejar las operaciones de acceso a datos para la entidad Reaccion
public class ReaccionRepository :
    GenericRepository<Reaccion>,
    IReaccionRepository
{
    public ReaccionRepository(NetForumDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<TipoReaccion>> ObtenerTiposAsync()
    {
        return await _context.TiposReaccion
            .AsNoTracking()
            .OrderBy(t => t.TipoReaccionId)
            .ToListAsync();
    }

    public async Task<Reaccion?> ObtenerAsync(
        int usuarioId,
        int temaId,
        int tipoReaccionId)
    {
        return await _context.Reacciones
            .FirstOrDefaultAsync(r =>
                r.UsuarioId == usuarioId &&
                r.TemaId == temaId &&
                r.TipoReaccionId == tipoReaccionId);
    }

    public async Task<IEnumerable<Reaccion>> ObtenerPorTemaAsync(
        int temaId)
    {
        return await _context.Reacciones
            .AsNoTracking()
            .Include(r => r.TipoReaccion)
            .Where(r => r.TemaId == temaId)
            .ToListAsync();
    }

    public async Task EliminarAsync(Reaccion reaccion)
    {
        _context.Reacciones.Remove(reaccion);
        await _context.SaveChangesAsync();
    }
}