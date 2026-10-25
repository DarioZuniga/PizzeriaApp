using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - La clase ReporteRepository tiene la �nica responsabilidad de manejar las operaciones de acceso a datos relacionadas con los reportes.
// Centraliza las consultas espec�ficas de reportes.
public class ReporteRepository : GenericRepository<Reporte>, IReporteRepository
{
    public ReporteRepository(PizzeriaAppDbContext context)
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