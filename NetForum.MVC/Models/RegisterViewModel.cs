using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de registro para la vista
public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(30, ErrorMessage = "El nombre de usuario no puede tener más de 30 caracteres.")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Por favor, introduce un correo electrónico válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe confirmar la contraseña.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}