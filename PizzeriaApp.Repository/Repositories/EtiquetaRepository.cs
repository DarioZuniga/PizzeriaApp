using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - la clase tiene una �nica responsabilidad: manejar las operaciones de acceso a datos para la entidad Etiqueta
public class EtiquetaRepository : GenericRepository<Etiqueta>, IEtiquetaRepository
{
    public EtiquetaRepository(PizzeriaAppDbContext context)
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