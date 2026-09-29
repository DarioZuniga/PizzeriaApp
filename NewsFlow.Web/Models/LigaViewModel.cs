using System.ComponentModel.DataAnnotations;

namespace NewsFlow.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de una liga para la vista
public class LigaViewModel
{
    [Required(ErrorMessage = "El nombre de la liga es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El país es obligatorio.")]
    [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres.")]
    public string Pais { get; set; } = string.Empty;
}