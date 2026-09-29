using Microsoft.EntityFrameworkCore;
using NewsFlow.Data;
using NewsFlow.Data.Entities;
using NewsFlow.Repository.Interfaces;

namespace NewsFlow.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - la clase tiene una única responsabilidad: manejar las operaciones de acceso a datos para la entidad Etiqueta
public class EtiquetaRepository : GenericRepository<Etiqueta>, IEtiquetaRepository
{
    public EtiquetaRepository(NewsFlowDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Etiqueta>> BuscarAsync(string filtro)
    {
        return await _context.Etiquetas
            .Where(e => e.Nombre.Contains(filtro))
            .OrderBy(e => e.Nombre)
            .ToListAsync();
    }
}