namespace NetForum.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de un tema del foro para la vista
public class ForoTemaViewModel
{
    public int TemaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string ContenidoResumen { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public int CantidadRespuestas { get; set; }

    public int CantidadReacciones { get; set; }
}