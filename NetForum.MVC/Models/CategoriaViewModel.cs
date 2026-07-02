using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models;

public class CategoriaViewModel
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;
}