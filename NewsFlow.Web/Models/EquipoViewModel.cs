using System.ComponentModel.DataAnnotations;

namespace NewsFlow.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de un equipo para la vista
public class EquipoViewModel
{
    [Required(ErrorMessage = "Debe seleccionar una liga.")]
    public int LigaId { get; set; }

    [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
    [StringLength(30, ErrorMessage = "El nombre no puede superar los 30 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El país es obligatorio.")]
    [StringLength(20, ErrorMessage = "El país no puede superar los 20 caracteres.")]
    public string Pais { get; set; } = string.Empty;

    [Range(1800, 2100, ErrorMessage = "Ingrese un año de fundación válido.")]
    public int Fundacion { get; set; }
}