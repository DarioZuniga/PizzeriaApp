using System.ComponentModel.DataAnnotations;

namespace NewsFlow.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de inicio de sesión para la vista
public class LoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool Recordarme { get; set; }
}