using System.ComponentModel.DataAnnotations;

namespace NewsFlow.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de una categoría para la vista
public class CategoriaViewModel
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;
}