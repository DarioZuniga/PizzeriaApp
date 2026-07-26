namespace NetForum.MVC.Models;

public class ForoRespuestaViewModel
{
    public int RespuestaId { get; set; }

    public string Contenido { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public bool EsAutorTema { get; set; }
}