using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de una categor�a para la vista
public class CategoriaViewModel
{
    [Required(ErrorMessage = "El nombre de la categor�a es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripci�n es obligatoria.")]
    [StringLength(300, ErrorMessage = "La descripci�n no puede superar los 300 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;
}