using System.ComponentModel.DataAnnotations;

namespace NewsFlow.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de una respuesta para la vista
public class RespuestaViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un tema.")]
    public int TemaId { get; set; }

    [Required(ErrorMessage = "El contenido de la respuesta es obligatorio.")]
    public string Contenido { get; set; } = string.Empty;
}