using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de registro para la vista
public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(30, ErrorMessage = "El nombre de usuario no puede tener m�s de 30 caracteres.")]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electr�nico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Por favor, introduce un correo electr�nico v�lido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contrase�a es obligatoria.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contrase�a debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe confirmar la contrase�a.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contrase�as no coinciden.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}