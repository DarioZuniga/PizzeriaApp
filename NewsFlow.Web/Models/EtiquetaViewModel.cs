using System.ComponentModel.DataAnnotations;

namespace NewsFlow.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de una etiqueta para la vista
public class EtiquetaViewModel
{
    [Required(ErrorMessage = "El nombre de la etiqueta es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}