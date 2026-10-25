using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de un reporte para la vista
public class ReporteViewModel
{
    public int ReporteId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un estado.")]
    [Range(1, int.MaxValue, ErrorMessage = "El estado seleccionado no es v�lido.")]
    public int EstadoId { get; set; }
}