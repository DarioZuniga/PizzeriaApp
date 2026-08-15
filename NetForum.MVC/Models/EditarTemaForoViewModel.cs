using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models;

public class EditarTemaForoViewModel
{
    public int TemaId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una categoría.")]
    [Range(1, int.MaxValue,
        ErrorMessage = "La categoría seleccionada no es válida.")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(
        200,
        MinimumLength = 5,
        ErrorMessage = "El título debe contener entre 5 y 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contenido es obligatorio.")]
    [StringLength(
        10000,
        MinimumLength = 10,
        ErrorMessage = "El contenido debe contener entre 10 y 10000 caracteres.")]
    public string Contenido { get; set; } = string.Empty;

    public List<int> EtiquetaIds { get; set; } = new();
}