using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Business.Models;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Repository.Interfaces;

namespace PizzeriaApp.Business.Services;

// DP: Service Layer Pattern
// SOLID: Dependency Inversion Principle - la clase depende de la abstracci�n
// IReaccionRepository, no de una implementaci�n concreta.
public class ReaccionService : IReaccionService
{
    private readonly IReaccionRepository _repository;
    private readonly ITemaRepository _temaRepository;

    public ReaccionService(
        IReaccionRepository repository,
        ITemaRepository temaRepository)
    {
        _repository = repository;
        _temaRepository = temaRepository;
    }

    public async Task<IEnumerable<TipoReaccion>> ObtenerTiposAsync()
    {
        return await _repository.ObtenerTiposAsync();
    }

    public async Task<IEnumerable<Reaccion>> ObtenerPorTemaAsync(
        int temaId)
    {
        return await _repository.ObtenerPorTemaAsync(temaId);
    }

    public async Task<ResultadoReaccion> AlternarReaccionAsync(
        int usuarioId,
        int temaId,
        int tipoReaccionId)
    {
        if (usuarioId <= 0)
        {
            throw new ArgumentException(
                "El usuario autenticado no es v�lido.",
                nameof(usuarioId));
        }

        if (temaId <= 0)
        {
            throw new ArgumentException(
                "El tema seleccionado no es v�lido.",
                nameof(temaId));
        }

        if (tipoReaccionId <= 0)
        {
            throw new ArgumentException(
                "El tipo de reacci�n no es v�lido.",
                nameof(tipoReaccionId));
        }

        var tema = await _temaRepository.GetByIdAsync(temaId);

        if (tema is null)
        {
            throw new KeyNotFoundException(
                "El tema seleccionado no existe.");
        }

        var tipos = await _repository.ObtenerTiposAsync();

        if (!tipos.Any(t =>
                t.TipoReaccionId == tipoReaccionId))
        {
            throw new InvalidOperationException(
                "El tipo de reacci�n seleccionado no existe.");
        }

        var reaccionExistente = await _repository.ObtenerAsync(
            usuarioId,
            temaId,
            tipoReaccionId);

        var agregada = reaccionExistente is null;

        if (agregada)
        {
            var reaccion = new Reaccion
            {
                UsuarioId = usuarioId,
                TemaId = temaId,
                TipoReaccionId = tipoReaccionId
            };

            await _repository.AddAsync(reaccion);
        }
        else
        {
            await _repository.EliminarAsync(reaccionExistente!);
        }

        var reacciones =
            await _repository.ObtenerPorTemaAsync(temaId);

        return new ResultadoReaccion
        {
            Agregada = agregada,
            Mensaje = agregada
                ? "Reacci�n agregada correctamente."
                : "Reacci�n eliminada correctamente.",
            TotalReacciones = reacciones.Count()
        };
    }
}