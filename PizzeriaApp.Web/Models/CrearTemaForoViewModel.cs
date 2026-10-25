using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;

public class CrearTemaForoViewModel
{
    [Required(ErrorMessage = "Debe seleccionar una categor�a.")]
    [Range(1, int.MaxValue, ErrorMessage = "La categor�a seleccionada no es v�lida.")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El t�tulo es obligatorio.")]
    [StringLength(
        200,
        MinimumLength = 5,
        ErrorMessage = "El t�tulo debe contener entre 5 y 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contenido es obligatorio.")]
    [StringLength(
        10000,
        MinimumLength = 10,
        ErrorMessage = "El contenido debe contener entre 10 y 10000 caracteres.")]
    public string Contenido { get; set; } = string.Empty;

    public List<int> EtiquetaIds { get; set; } = new();
}