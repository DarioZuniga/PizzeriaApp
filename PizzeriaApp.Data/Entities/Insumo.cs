namespace PizzeriaApp.Data.Entities;

public class Insumo
{
    public int InsumoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string UnidadMedida { get; set; } = string.Empty; // Ej: kg, litros, unidades
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public decimal CostoUnitario { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public bool Activo { get; set; } = true;
    
    public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
    public ICollection<RecetaIngrediente> RecetaIngredientes { get; set; } = new List<RecetaIngrediente>();
}