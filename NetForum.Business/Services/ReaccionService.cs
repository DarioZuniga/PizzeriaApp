using NetForum.Business.Interfaces;
using NetForum.Business.Models;
using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;

namespace NetForum.Business.Services;

// DP: Service Layer Pattern
// SOLID: Single Responsibility Principle
public class ReaccionService : IReaccionService
{
    private readonly IReaccionRepository _repository;
    private readonly ITemaRepository _temaRepository;

    public ReaccionService(
        IReaccionRepository repository,
        ITemaRepository temaRepository)
    {
        // SOLID: Dependency Inversion Principle
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
                "El usuario autenticado no es válido.",
                nameof(usuarioId));
        }

        if (temaId <= 0)
        {
            throw new ArgumentException(
                "El tema seleccionado no es válido.",
                nameof(temaId));
        }

        if (tipoReaccionId <= 0)
        {
            throw new ArgumentException(
                "El tipo de reacción no es válido.",
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
                "El tipo de reacción seleccionado no existe.");
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
                ? "Reacción agregada correctamente."
                : "Reacción eliminada correctamente.",
            TotalReacciones = reacciones.Count()
        };
    }
}