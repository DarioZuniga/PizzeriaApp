using System.ComponentModel.DataAnnotations;

namespace NewsFlow.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de un reporte para la vista
public class CrearReporteViewModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El tema seleccionado no es válido.")]
    public int TemaId { get; set; }

    [Required(ErrorMessage = "Debe indicar el motivo del reporte.")]
    [StringLength(
        500,
        MinimumLength = 10,
        ErrorMessage = "El motivo debe contener entre 10 y 500 caracteres.")]
    public string Motivo { get; set; } = string.Empty;
}