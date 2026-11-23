namespace PizzeriaApp.MVC.Models;

public class CarritoItem
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioBase { get; set; }
    public int Cantidad { get; set; } = 1;
    
    public int? TamanoId { get; set; }
    public string? TamanoNombre { get; set; }
    public decimal PrecioTamano { get; set; } // Guarda el precio calculado del tamaño
    
    public int? TipoMasaId { get; set; }
    public string? TipoMasaNombre { get; set; }
    public decimal PrecioMasa { get; set; } // Guarda el precio de la masa
    
    public List<int> IngredientesIds { get; set; } = new();
    public List<string> IngredientesNombres { get; set; } = new();
    public decimal PrecioIngredientes { get; set; } // Suma de ingredientes
    
    public string? Notas { get; set; }
    
    // El precio final se calcula sumando todo
    public decimal PrecioTotal => (PrecioBase + PrecioTamano + PrecioMasa + PrecioIngredientes) * Cantidad;
}