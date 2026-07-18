using NetForum.Business.Interfaces;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
public class RespuestaService : IRespuestaService
{
    private readonly IRespuestaRepository _repository;

    public RespuestaService(IRespuestaRepository repository)
    {
        // SOLID: Dependency Inversion Principle
        _repository = repository;
    }

    public async Task<IEnumerable<Respuesta>> ObtenerRespuestasAsync()
    {
        return await _repository.ObtenerConRelacionesAsync();
    }

    public async Task<IEnumerable<Respuesta>> BuscarRespuestasAsync(string filtro)
    {
        return await _repository.BuscarAsync(filtro);
    }

    public async Task<Respuesta> GuardarAsync(Respuesta respuesta)
    {
        respuesta.FechaCreacion = DateTime.Now;
        return await _repository.AddAsync(respuesta);
    }

    public async Task ActualizarAsync(Respuesta respuesta)
    {
        await _repository.UpdateAsync(respuesta);
    }

    public async Task EliminarAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Respuesta?> ObtenerPorIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}