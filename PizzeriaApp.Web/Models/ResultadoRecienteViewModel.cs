namespace PizzeriaApp.MVC.Models;

public class ResultadoRecienteViewModel
{
    public int PartidoId { get; set; }

    public string Competicion { get; set; } = string.Empty;

    public string Local { get; set; } = string.Empty;

    public string Visitante { get; set; } = string.Empty;

    public string? EscudoLocal { get; set; }

    public string? EscudoVisitante { get; set; }

    public int GolesLocal { get; set; }

    public int GolesVisitante { get; set; }

    public DateTime FechaUtc { get; set; }
}