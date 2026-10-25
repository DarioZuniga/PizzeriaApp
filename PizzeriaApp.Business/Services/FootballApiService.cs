using System.Net.Http.Json;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Business.Models.Football;

namespace PizzeriaApp.Business.Services;

public class FootballApiService : IFootballApiService // btener los pr�ximos partidos de la API
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

    /*Funciones principales del c�digo

        Inyecci�n de HttpClient:
        En el constructor recibe una instancia de HttpClient ya configurada (que previamente tiene la URL base y la API Key de football-data.org).

        Petici�n HTTP AJAX / JSON:
        Mediante _httpClient.GetFromJsonAsync<FootballMatchesResponse>("matches"), realiza una solicitud GET al endpoint /matches y desglosa (deserializa) la respuesta de la API directamente a un objeto C#.

        Control de nulos:
        Si la API no responde o la lista de partidos viene vac�a (response?.Matches is null), retorna una lista vac�a para evitar errores en la aplicaci�n.

        Filtrado y Procesamiento (LINQ):
        Aplica tres reglas a los datos recibidos:

        Where(...): Selecciona �nicamente los partidos cuya fecha sea futura (m.UtcDate >= DateTime.UtcNow) y cuyo estado sea programado (SCHEDULED o TIMED).

        OrderBy(...): Ordena los partidos cronol�gicamente, desde el m�s cercano en el tiempo hasta el m�s lejano.

        Take(cantidad): Limita el n�mero de resultados seg�n el par�metro cantidad (por defecto, toma los 5 partidos m�s pr�ximos).*/

    public async Task<IEnumerable<FootballMatchDto>>
    ObtenerUltimosResultadosAsync(int cantidad = 5)      // Dashboad resultados - API 
    {
        var response =
            await _httpClient
                .GetFromJsonAsync<FootballMatchesResponse>(
                    "competitions/CL/matches?status=FINISHED");

        if (response?.Matches is null)
        {
            return Enumerable.Empty<FootballMatchDto>();
        }

        return response.Matches
            .Where(m =>
                m.Status == "FINISHED" &&
                m.UtcDate <= DateTime.UtcNow)
            .OrderByDescending(m => m.UtcDate)
            .Take(cantidad)
            .ToList();
    }



}

