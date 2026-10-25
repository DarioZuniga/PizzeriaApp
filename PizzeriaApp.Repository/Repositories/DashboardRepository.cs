//Este repositorio ser� el �nico responsable de consultar las estad�sticas del Dashboard.

//Aqu� no se devuelve entidades completas, sino �nicamente los datos que necesitamos
// se utiliza el (CountAsync()), lo que mejora el rendimiento y mantiene la l�gica de acceso a datos

using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - esta clase tiene la �nica responsabilidad
// de consultar estad�sticas para el Dashboard.
public class DashboardRepository : IDashboardRepository
{
    private readonly PizzeriaAppDbContext _context;

    // SOLID: Dependency Injection - Se inyecta el contexto de la base de datos a trav�s del constructor,
    // lo que permite una mayor flexibilidad y facilita las pruebas unitarias.
    public DashboardRepository(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> ContarUsuariosAsync()
    {
        return await _context.Usuarios.CountAsync();
    }

    public async Task<int> ContarTemasAsync()
    {
        return await _context.Temas.CountAsync();
    }

    public async Task<int> ContarRespuestasAsync()
    {
        return await _context.Respuestas.CountAsync();
    }

    public async Task<int> ContarEquiposAsync()
    {
        return await _context.Equipos.CountAsync();
    }

    public async Task<int> ContarLigasAsync()
    {
        return await _context.Ligas.CountAsync();
    }
    public async Task<IEnumerable<Tema>> ObtenerTemasDestacadosAsync() // m�todo para obtener los temas m�s destacados
                                                                       // seg�n la cantidad de respuestas y reacciones
    {
        return await _context.Temas // consultar la tabla de Temas
            .AsNoTracking() // no se necesita seguimiento de cambios, ya que solo se van a leer los datos
            .Include(t => t.Respuestas)
            .Include(t => t.Reacciones)
            .OrderByDescending(t => // ordenar por la suma de respuestas y reacciones
                t.Respuestas.Count + t.Reacciones.Count)
            .ThenByDescending(t => t.FechaCreacion) // en caso de empate, ordenar por fecha de creaci�n
            .Take(4) // tomar solo los 3 primeros resultados
            .ToListAsync(); // ejecutar la consulta de manera as�ncrona y devolver la lista de temas
    }
    public async Task<Dictionary<string, int>> // m�todo para obtener la cantidad de temas por categor�a en el dashboard
    ObtenerTemasPorCategoriaAsync()
    {
        var datos = await _context.Temas
            .AsNoTracking()
            .GroupBy(t => t.Categoria.Nombre)
            .Select(grupo => new
            {
                Categoria = grupo.Key,
                Cantidad = grupo.Count()
            })
            .OrderByDescending(x => x.Cantidad)
            .ThenBy(x => x.Categoria)
            .ToListAsync();

        return datos.ToDictionary(
            x => x.Categoria,
            x => x.Cantidad);
    }

    public async Task<Dictionary<DateTime, int>> ObtenerRespuestasPorDiaAsync(int dias = 7)
    {
        if (dias <= 0)
        {
            dias = 7;
        }

        var hoy = DateTime.Today;

        var fechaInicio = hoy.AddDays(-(dias - 1));

        var fechaFin = hoy.AddDays(1);

        var datos = await _context.Respuestas
            .AsNoTracking()
            .Where(r =>
                r.FechaCreacion >= fechaInicio &&
                r.FechaCreacion < fechaFin)
            .GroupBy(r => r.FechaCreacion.Date)
            .Select(grupo => new
            {
                Fecha = grupo.Key,
                Cantidad = grupo.Count()
            })
            .ToListAsync();

        var resultado =
            new Dictionary<DateTime, int>();

        for (var fecha = fechaInicio;
             fecha <= hoy;
             fecha = fecha.AddDays(1))
        {
            var cantidad = datos
                .FirstOrDefault(x => x.Fecha == fecha)
                ?.Cantidad ?? 0;

            resultado.Add(fecha, cantidad);
        }

        return resultado;
    }
}