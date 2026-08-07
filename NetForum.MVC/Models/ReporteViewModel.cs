using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de un reporte para la vista
public class ReporteViewModel
{
    public int ReporteId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un estado.")]
    [Range(1, int.MaxValue, ErrorMessage = "El estado seleccionado no es válido.")]
    public int EstadoId { get; set; }
}