namespace PizzeriaApp.Business.Models.Football;

public class FootballCompetitionDto // metodo para representar una competici�n de f�tbol en la API de partidos de f�tbol
{
    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Emblem { get; set; }
}