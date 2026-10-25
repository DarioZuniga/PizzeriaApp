namespace PizzeriaApp.MVC.Models;
// SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la informaci�n de un tipo de reacci�n para la vista
public class ForoTipoReaccionViewModel
{
    public int TipoReaccionId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public bool SeleccionadaPorUsuario { get; set; }
}