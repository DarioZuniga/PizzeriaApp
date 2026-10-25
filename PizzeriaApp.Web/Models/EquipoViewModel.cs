using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de un equipo para la vista
public class EquipoViewModel
{
    [Required(ErrorMessage = "Debe seleccionar una liga.")]
    public int LigaId { get; set; }

    [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
    [StringLength(30, ErrorMessage = "El nombre no puede superar los 30 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El pa�s es obligatorio.")]
    [StringLength(20, ErrorMessage = "El pa�s no puede superar los 20 caracteres.")]
    public string Pais { get; set; } = string.Empty;

    [Range(1800, 2100, ErrorMessage = "Ingrese un a�o de fundaci�n v�lido.")]
    public int Fundacion { get; set; }
}