namespace NetForum.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de un tema del foro para la vista
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

    public IEnumerable<ForoRespuestaViewModel> Respuestas { get; set; } // su función es almacenar una lista de respuestas
                                                                        // asociadas al tema del foro
        = Enumerable.Empty<ForoRespuestaViewModel>();
    public IEnumerable<ForoTipoReaccionViewModel> Reacciones { get; set; }// su función es almacenar una lista de tipos de reacciones
                                                                          // asociadas al tema del foro
    = Enumerable.Empty<ForoTipoReaccionViewModel>();

}