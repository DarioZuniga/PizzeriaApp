namespace PizzeriaApp.Business.Models.Football;

public class FootballMatchesResponse // modelo de respuesta para la API de partidos de f�tbol
{
    public List<FootballMatchDto> Matches { get; set; } = new();
}