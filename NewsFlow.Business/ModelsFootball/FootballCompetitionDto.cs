namespace NewsFlow.Business.Models.Football;

public class FootballCompetitionDto // metodo para representar una competición de fútbol en la API de partidos de fútbol
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Emblem { get; set; }
}