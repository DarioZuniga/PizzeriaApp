using System.Net.Http.Json;
using NetForum.Business.Interfaces;
using NetForum.Business.Models.Football;

namespace NetForum.Business.Services;

public class FootballApiService : IFootballApiService // btener los próximos partidos de la API
{
    private readonly HttpClient _httpClient;

    public FootballApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<FootballMatchDto>>
        ObtenerProximosPartidosAsync(int cantidad = 5)
    {
        var response =
     await _httpClient.GetFromJsonAsync<FootballMatchesResponse>(
         "competitions/PL/matches");

        if (response?.Matches is null)
        {
            return Enumerable.Empty<FootballMatchDto>();
        }

        return response.Matches
            .Where(m =>
                m.UtcDate >= DateTime.UtcNow &&
                (
                    m.Status == "SCHEDULED" ||
                    m.Status == "TIMED"
                ))
            .OrderBy(m => m.UtcDate)
            .Take(cantidad)
            .ToList();
    }
}

/*Funciones principales del código

    Inyección de HttpClient:
    En el constructor recibe una instancia de HttpClient ya configurada (que previamente tiene la URL base y la API Key de football-data.org).

    Petición HTTP AJAX / JSON:
    Mediante _httpClient.GetFromJsonAsync<FootballMatchesResponse>("matches"), realiza una solicitud GET al endpoint /matches y desglosa (deserializa) la respuesta de la API directamente a un objeto C#.

    Control de nulos:
    Si la API no responde o la lista de partidos viene vacía (response?.Matches is null), retorna una lista vacía para evitar errores en la aplicación.

    Filtrado y Procesamiento (LINQ):
    Aplica tres reglas a los datos recibidos:

    Where(...): Selecciona únicamente los partidos cuya fecha sea futura (m.UtcDate >= DateTime.UtcNow) y cuyo estado sea programado (SCHEDULED o TIMED).

    OrderBy(...): Ordena los partidos cronológicamente, desde el más cercano en el tiempo hasta el más lejano.

    Take(cantidad): Limita el número de resultados según el parámetro cantidad (por defecto, toma los 5 partidos más próximos).*/