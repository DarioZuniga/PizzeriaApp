using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - la clase tiene una �nica responsabilidad: manejar las operaciones de acceso a datos para la entidad Equipo
public class EquipoRepository : GenericRepository<Equipo>, IEquipoRepository
{
    public EquipoRepository(PizzeriaAppDbContext context)
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