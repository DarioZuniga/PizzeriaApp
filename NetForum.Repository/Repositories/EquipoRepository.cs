using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - la clase tiene una única responsabilidad: manejar las operaciones de acceso a datos para la entidad Equipo
public class EquipoRepository : GenericRepository<Equipo>, IEquipoRepository
{
    public EquipoRepository(NetForumDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Equipo>> ObtenerConLigaAsync()
    {
        return await _context.Equipos
            .Include(e => e.Liga)
            .OrderBy(e => e.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Equipo>> BuscarAsync(string filtro)
    {
        return await _context.Equipos
            .Include(e => e.Liga)
            .Where(e =>
                e.Nombre.Contains(filtro) ||
                e.Pais.Contains(filtro) ||
                e.Liga.Nombre.Contains(filtro))
            .OrderBy(e => e.Nombre)
            .ToListAsync();
    }
}