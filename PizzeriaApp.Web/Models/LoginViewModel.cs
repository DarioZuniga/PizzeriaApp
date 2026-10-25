using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de inicio de sesi�n para la vista
public class LoginViewModel
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo v�lido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contrase�a es obligatoria.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool Recordarme { get; set; }
}