using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de un tema para la vista
public class TemaViewModel
{
    [Required(ErrorMessage = "Debe seleccionar una categoría.")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(200, ErrorMessage = "El título no puede superar los 200 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contenido es obligatorio.")]
    public string Contenido { get; set; } = string.Empty;
}