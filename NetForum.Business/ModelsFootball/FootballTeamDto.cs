namespace NetForum.Business.Models.Football;

public class FootballTeamDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ShortName { get; set; } = string.Empty;

    public string? Crest { get; set; }
}