using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle
public class EtiquetaRepository : GenericRepository<Etiqueta>, IEtiquetaRepository
{
    public EtiquetaRepository(NetForumDbContext context)
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