namespace PizzeriaApp.MVC.Models
{
    // SOLID: Single Responsibility Principle - esta clase solo se encarga de representar la información de un error para la vista
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}