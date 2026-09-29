namespace NewsFlow.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de una respuesta del foro para la vista
public class ForoRespuestaViewModel
{
    public int RespuestaId { get; set; }

    public string Contenido { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public bool EsAutorTema { get; set; }
}