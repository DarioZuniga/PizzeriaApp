using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models;

public class EtiquetaViewModel
{
    [Required(ErrorMessage = "El nombre de la etiqueta es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}