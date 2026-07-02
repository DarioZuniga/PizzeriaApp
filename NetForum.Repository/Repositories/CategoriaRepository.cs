using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - maneja consultas específicas de Categoría.
public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(NetForumDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Categoria>> ObtenerCategoriasActivasAsync()
    {
        return await _context.Categorias
            .Include(c => c.Estado)
            .Where(c => c.EstadoId == 1)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<Categoria>> BuscarPorNombreAsync(string nombre)
    {
        return await _context.Categorias
            .Include(c => c.Estado)
            .Where(c => c.Nombre.Contains(nombre))
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }
}