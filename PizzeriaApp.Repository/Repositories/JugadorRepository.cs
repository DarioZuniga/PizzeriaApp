using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - esta clase tiene la responsabilidad �nica de manejar las operaciones de acceso a datos para la entidad Jugador
public class JugadorRepository : GenericRepository<Jugador>, IJugadorRepository
{
    public JugadorRepository(PizzeriaAppDbContext context)
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