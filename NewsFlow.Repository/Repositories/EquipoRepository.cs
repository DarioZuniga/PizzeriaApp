using Microsoft.EntityFrameworkCore;
using NewsFlow.Data;
using NewsFlow.Data.Entities;
using NewsFlow.Repository.Interfaces;

namespace NewsFlow.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - la clase tiene una única responsabilidad: manejar las operaciones de acceso a datos para la entidad Equipo
public class EquipoRepository : GenericRepository<Equipo>, IEquipoRepository
{
    public EquipoRepository(NewsFlowDbContext context)
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