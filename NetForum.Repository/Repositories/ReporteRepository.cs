using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle
// Centraliza las consultas específicas de reportes.
public class ReporteRepository : GenericRepository<Reporte>, IReporteRepository
{
    public ReporteRepository(NetForumDbContext context)
        : base(context)
    {
    }
    public async Task<IEnumerable<Reporte>> ObtenerConRelacionesAsync()
    {
        return await _context.Reportes
            .Include(r => r.Usuario)
            .Include(r => r.Tema)
            .Include(r => r.Estado)
            .OrderByDescending(r => r.FechaReporte)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reporte>> BuscarAsync(string filtro)
    {
        return await _context.Reportes
            .Include(r => r.Usuario)
            .Include(r => r.Tema)
            .Include(r => r.Estado)
            .Where(r =>
                r.Motivo.Contains(filtro) ||
                r.Tema.Titulo.Contains(filtro) ||
                r.Usuario.NombreUsuario.Contains(filtro) ||
                r.Estado.Nombre.Contains(filtro))
            .OrderByDescending(r => r.FechaReporte)
            .ToListAsync();
    }
    public async Task<IEnumerable<Estado>> ObtenerEstadosAsync()
    {
        return await _context.Estados
            .OrderBy(e => e.Nombre)
            .ToListAsync();
    }

    public async Task<Estado?> ObtenerEstadoInicialAsync()
    {
        var estadoActivo = await _context.Estados
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Nombre == "Activo");

        if (estadoActivo is not null)
        {
            return estadoActivo;
        }

        return await _context.Estados
            .AsNoTracking()
            .OrderBy(e => e.EstadoId)
            .FirstOrDefaultAsync();
    }
}