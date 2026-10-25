namespace PizzeriaApp.MVC.Models;

public class PerfilTemaViewModel
{
    public int TemaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public int CantidadRespuestas { get; set; }

    public int CantidadReacciones { get; set; }
}
