namespace NewsFlow.MVC.Models;

public class PerfilUsuarioViewModel
{
    public int UsuarioId { get; set; }

    public string NombreUsuario { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }

    public int TotalTemas { get; set; }

    public int TotalRespuestas { get; set; }

    public int TotalReacciones { get; set; }

    public IEnumerable<PerfilTemaViewModel> Temas { get; set; }
        = Enumerable.Empty<PerfilTemaViewModel>();
}