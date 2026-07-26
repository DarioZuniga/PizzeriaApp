namespace NetForum.MVC.Models;

public class ForoDetalleViewModel
{
    public int TemaId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Contenido { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public int CantidadReacciones { get; set; }

    public IEnumerable<ForoRespuestaViewModel> Respuestas { get; set; }
        = Enumerable.Empty<ForoRespuestaViewModel>();
}