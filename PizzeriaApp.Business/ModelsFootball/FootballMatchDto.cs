namespace PizzeriaApp.Business.Models.Football;

public class FootballMatchDto // metodo para representar un partido de f�tbol en la API de partidos de f�tbol
{
    public int Id { get; set; }

    public DateTime UtcDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public FootballCompetitionDto Competition { get; set; } = new();

    public FootballTeamDto HomeTeam { get; set; } = new();

    public FootballTeamDto AwayTeam { get; set; } = new();
    public FootballScoreDto Score { get; set; } = new();
}