using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de un jugador para la vista
public class JugadorViewModel
{
    [Required(ErrorMessage = "Debe seleccionar un equipo.")]
    public int EquipoId { get; set; }

    [Required(ErrorMessage = "El nombre del jugador es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La posici�n es obligatoria.")]
    [StringLength(50, ErrorMessage = "La posici�n no puede superar los 50 caracteres.")]
    public string Posicion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    [StringLength(50, ErrorMessage = "La nacionalidad no puede superar los 50 caracteres.")]
    public string Nacionalidad { get; set; } = string.Empty;
}