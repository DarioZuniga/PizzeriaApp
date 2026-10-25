using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de una respuesta para la vista
public class CrearRespuestaViewModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El tema seleccionado no es v�lido.")]
    public int TemaId { get; set; }

    [Required(ErrorMessage = "Debe escribir una respuesta.")]
    [StringLength(
        3000,
        MinimumLength = 2,
        ErrorMessage = "La respuesta debe contener entre 2 y 3000 caracteres.")]
    public string Contenido { get; set; } = string.Empty;
}