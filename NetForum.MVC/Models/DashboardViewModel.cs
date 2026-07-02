namespace NetForum.MVC.Models;

// SOLID: Single Responsibility Principle
// Este ViewModel únicamente transporta información hacia la vista.

public class DashboardViewModel
{
    public int TotalUsuarios { get; set; }

    public int TotalTemas { get; set; }

    public int TotalRespuestas { get; set; }

    public int TotalEquipos { get; set; }

    public int TotalLigas { get; set; }

    public string NombreUsuario { get; set; } = string.Empty;
}