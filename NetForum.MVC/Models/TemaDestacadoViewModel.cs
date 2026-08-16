namespace NetForum.MVC.Models;

public class TemaDestacadoViewModel
{
    public int TemaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Resumen { get; set; } = string.Empty;

    public string Iniciales { get; set; } = string.Empty;

    public int CantidadRespuestas { get; set; }

    public int CantidadReacciones { get; set; }

    public DateTime FechaCreacion { get; set; }
}