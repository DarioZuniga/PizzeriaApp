using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - maneja consultas espec�ficas de Categor�a.
public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(PizzeriaAppDbContext context)
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

    public async Task<bool> TieneTemasAsociadosAsync(int categoriaId)
    {
        return await _context.Temas
            .AnyAsync(t => t.CategoriaId == categoriaId);
    }
}