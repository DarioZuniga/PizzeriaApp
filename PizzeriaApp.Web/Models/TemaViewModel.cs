using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de un tema para la vista
public class TemaViewModel
{
    [Required(ErrorMessage = "Debe seleccionar una categor�a.")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El t�tulo es obligatorio.")]
    [StringLength(200, ErrorMessage = "El t�tulo no puede superar los 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contenido es obligatorio.")]
    public string Contenido { get; set; } = string.Empty;
}