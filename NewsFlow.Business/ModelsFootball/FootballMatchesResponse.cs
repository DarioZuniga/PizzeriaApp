namespace NewsFlow.Business.Models.Football;

public class FootballMatchesResponse // modelo de respuesta para la API de partidos de fútbol
{
    public List<FootballMatchDto> Matches { get; set; } = new();
}