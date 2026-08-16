//Este repositorio será el único responsable de consultar las estadísticas del Dashboard.

//Aquí no se devuelve entidades completas, sino únicamente los datos que necesitamos
// se utiliza el (CountAsync()), lo que mejora el rendimiento y mantiene la lógica de acceso a datos

using Microsoft.EntityFrameworkCore;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Repository.Repositories;

// DP: Repository Pattern
// SOLID: Single Responsibility Principle - esta clase tiene la única responsabilidad
// de consultar estadísticas para el Dashboard.
public class DashboardRepository : IDashboardRepository
{
    private readonly NetForumDbContext _context;

    // SOLID: Dependency Injection - Se inyecta el contexto de la base de datos a través del constructor,
    // lo que permite una mayor flexibilidad y facilita las pruebas unitarias.
    public DashboardRepository(NetForumDbContext context)
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
    public async Task<IEnumerable<Tema>> ObtenerTemasDestacadosAsync() // método para obtener los temas más destacados
                                                                       // según la cantidad de respuestas y reacciones
    {
        return await _context.Temas // consultar la tabla de Temas
            .AsNoTracking() // no se necesita seguimiento de cambios, ya que solo se van a leer los datos
            .Include(t => t.Respuestas)
            .Include(t => t.Reacciones)
            .OrderByDescending(t => // ordenar por la suma de respuestas y reacciones
                t.Respuestas.Count + t.Reacciones.Count)
            .ThenByDescending(t => t.FechaCreacion) // en caso de empate, ordenar por fecha de creación
            .Take(4) // tomar solo los 3 primeros resultados
            .ToListAsync(); // ejecutar la consulta de manera asíncrona y devolver la lista de temas
    }
}