namespace NewsFlow.MVC.Models;
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
    public IEnumerable<ForoRespuestaViewModel> Respuestas { get; set; } // Almacenar una lista de respuestas
                                                                        // asociadas al tema del foro
        = Enumerable.Empty<ForoRespuestaViewModel>();
    public IEnumerable<ForoTipoReaccionViewModel> Reacciones { get; set; }// Almacenar una lista de tipos de reacciones
                                                                          // asociadas al tema del foro
    = Enumerable.Empty<ForoTipoReaccionViewModel>();
    public IEnumerable<ForoEtiquetaViewModel> Etiquetas { get; set; } // Almacenar una lista de etiquetas
                                                                      // asociadas al tema del foro
    = Enumerable.Empty<ForoEtiquetaViewModel>();
    public bool EsPropietario { get; set; } // botón DE Editar solamente al propietario

}