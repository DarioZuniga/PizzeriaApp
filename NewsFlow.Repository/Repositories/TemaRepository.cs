using Microsoft.EntityFrameworkCore;
using NewsFlow.Data;
using NewsFlow.Data.Entities;
using NewsFlow.Repository.Interfaces;

namespace NewsFlow.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - aplica principalmente: la clase centraliza
// el acceso a datos de Tema. Se mantiene así por simplicidad del feed
// del foro, que necesita categorías junto con temas en la misma consulta.
public class TemaRepository : GenericRepository<Tema>, ITemaRepository
{
    public TemaRepository(NewsFlowDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Tema>> ObtenerConRelacionesAsync() 
    {
        return await _context.Temas
            .Include(t => t.Usuario)
            .Include(t => t.Categoria)
            .Include(t => t.Estado)
            .OrderByDescending(t => t.FechaCreacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tema>> BuscarAsync(string filtro)
    {
        return await _context.Temas
            .Include(t => t.Usuario)
            .Include(t => t.Categoria)
            .Include(t => t.Estado)
            .Where(t =>
                t.Titulo.Contains(filtro) ||
                t.Contenido.Contains(filtro) ||
                t.Categoria.Nombre.Contains(filtro) ||
                t.Usuario.NombreUsuario.Contains(filtro))
            .OrderByDescending(t => t.FechaCreacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tema>> ObtenerFeedAsync(  // task que devuelve los temas para el feed del foro,
                                                            // con soporte para paginación y filtrado por categoría o etiqueta.
    string? buscar,
    int? categoriaId,
    int? etiquetaId)
    {
        var query = _context.Temas
            .AsNoTracking()
            .Include(t => t.Usuario)
            .Include(t => t.Categoria)
            .Include(t => t.Estado)
            .Include(t => t.Respuestas)
            .Include(t => t.Reacciones)
            .Include(t => t.TemaEtiquetas)
                .ThenInclude(te => te.Etiqueta)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar)) // si el parámetro de búsqueda no es nulo o vacío,
                                                // se aplica un filtro adicional a la consulta
        {
            var filtro = buscar.Trim();

            query = query.Where(t => // se filtra por título o contenido del tema
                t.Titulo.Contains(filtro) ||
                t.Contenido.Contains(filtro));
        }

        if (categoriaId.HasValue &&  // filto por categoría si se proporciona un Id de categoría válido
            categoriaId.Value > 0)
        {
            query = query.Where(t =>
                t.CategoriaId == categoriaId.Value);
        }

        if (etiquetaId.HasValue && // filto por etiqueta si se proporciona un Id de etiqueta válido
            etiquetaId.Value > 0)
        {
            query = query.Where(t =>
                t.TemaEtiquetas.Any(te =>
                    te.EtiquetaId == etiquetaId.Value));
        }

        return await query // se ejecuta la consulta y se devuelve la lista de temas resultante
            .OrderByDescending(t => t.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Tema?> ObtenerDetalleAsync(int temaId) // método para obtener un tema específico con
                                                             // todas sus relaciones, incluyendo usuario,
                                                             // categoría, estado, respuestas y etiquetas.
    {
   
     return await _context.Temas // se obtiene el tema con el Id especificado, incluyendo todas sus relaciones
    .AsNoTracking()
    .Include(t => t.Usuario) // se incluyen las relaciones
    .Include(t => t.Categoria)
    .Include(t => t.Estado) 
    .Include(t => t.Respuestas)
    .ThenInclude(r => r.Usuario)
    .Include(t => t.Respuestas)
    .ThenInclude(r => r.Estado)
    .Include(t => t.Reacciones)
    .Include(t => t.TemaEtiquetas)
    .ThenInclude(te => te.Etiqueta)
    .FirstOrDefaultAsync(t => t.TemaId == temaId);
    }

    public async Task<IEnumerable<Categoria>> ObtenerCategoriasAsync() // método para obtener todas las categorías
                                                                       // disponibles en el foro, ordenadas alfabéticamente por nombre.
    {
        return await _context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
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
