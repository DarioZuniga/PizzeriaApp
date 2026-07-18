using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models;

public class RespuestaViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un tema.")]
    public int TemaId { get; set; }

    [Required(ErrorMessage = "El contenido de la respuesta es obligatorio.")]
    public string Contenido { get; set; } = string.Empty;
}