//Este repositorio será el único responsable de consultar las estadísticas del Dashboard.

//Aquí no se devuelve entidades completas, sino únicamente los datos que necesitamos
// se utiliza el (CountAsync()), lo que mejora el rendimiento y mantiene la lógica de acceso a datos

using Microsoft.EntityFrameworkCore;
using NetForum.Data;
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
}