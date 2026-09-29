using Microsoft.EntityFrameworkCore;
using NewsFlow.Data;
using NewsFlow.Data.Entities;
using NewsFlow.Repository.Interfaces;

namespace NewsFlow.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle
public class TemaEtiquetaRepository : ITemaEtiquetaRepository
{
    private readonly NewsFlowDbContext _context;

    public TemaEtiquetaRepository(NewsFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Etiqueta>> ObtenerEtiquetasAsync()
    {
        return await _context.Etiquetas
            .AsNoTracking()
            .OrderBy(e => e.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<TemaEtiqueta>> ObtenerPorTemaAsync(
        int temaId)
    {
        return await _context.TemaEtiquetas
            .AsNoTracking()
            .Include(te => te.Etiqueta)
            .Where(te => te.TemaId == temaId)
            .ToListAsync();
    }

    public async Task ReemplazarEtiquetasAsync(
        int temaId,
        IEnumerable<int> etiquetaIds)
    {
        var actuales = await _context.TemaEtiquetas
            .Where(te => te.TemaId == temaId)
            .ToListAsync();

        _context.TemaEtiquetas.RemoveRange(actuales);

        var ids = etiquetaIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();

        foreach (var etiquetaId in ids)
        {
            _context.TemaEtiquetas.Add(
                new TemaEtiqueta
                {
                    TemaId = temaId,
                    EtiquetaId = etiquetaId
                });
        }

        await _context.SaveChangesAsync();
    }
}