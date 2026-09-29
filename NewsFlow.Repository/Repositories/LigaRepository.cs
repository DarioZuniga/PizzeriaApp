using Microsoft.EntityFrameworkCore;
using NewsFlow.Data;
using NewsFlow.Data.Entities;
using NewsFlow.Repository.Interfaces;

namespace NewsFlow.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - La clase LigaRepository tiene la responsabilidad única de manejar las operaciones de acceso a datos relacionadas con la entidad Liga.
public class LigaRepository : GenericRepository<Liga>, ILigaRepository
{
    public LigaRepository(NewsFlowDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Liga>> BuscarPorNombreOPaisAsync(string filtro)
    {
        return await _context.Ligas
            .Where(l => l.Nombre.Contains(filtro) || l.Pais.Contains(filtro))
            .OrderBy(l => l.Nombre)
            .ToListAsync();
    }
}