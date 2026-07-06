using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle
public class JugadorRepository : GenericRepository<Jugador>, IJugadorRepository
{
    public JugadorRepository(NetForumDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Jugador>> ObtenerConEquipoAsync()
    {
        return await _context.Jugadores
            .Include(j => j.Equipo)
            .OrderBy(j => j.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Jugador>> BuscarAsync(string filtro)
    {
        return await _context.Jugadores
            .Include(j => j.Equipo)
            .Where(j =>
                j.Nombre.Contains(filtro) ||
                j.Posicion.Contains(filtro) ||
                j.Nacionalidad.Contains(filtro) ||
                j.Equipo.Nombre.Contains(filtro))
            .OrderBy(j => j.Nombre)
            .ToListAsync();
    }
}