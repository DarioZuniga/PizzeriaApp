namespace PizzeriaApp.MVC.Models;

public class CarritoItem
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioBase { get; set; }
    public int Cantidad { get; set; } = 1;
    public int? TamanoId { get; set; }
    public string? TamanoNombre { get; set; }
    public decimal PrecioTamano { get; set; }
    public int? TipoMasaId { get; set; }
    public string? TipoMasaNombre { get; set; }
    public decimal PrecioMasa { get; set; }
    public List<int> IngredientesIds { get; set; } = new();
    public List<string> IngredientesNombres { get; set; } = new();
    public decimal PrecioIngredientes { get; set; }
    
    // El precio final se calcula con los valores reales que vienen de la BD
    public decimal PrecioTotal => (PrecioBase + PrecioTamano + PrecioMasa + PrecioIngredientes) * Cantidad;
}