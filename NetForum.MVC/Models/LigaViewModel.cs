using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models;

public class LigaViewModel
{
    [Required(ErrorMessage = "El nombre de la liga es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El país es obligatorio.")]
    [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres.")]
    public string Pais { get; set; } = string.Empty;
}