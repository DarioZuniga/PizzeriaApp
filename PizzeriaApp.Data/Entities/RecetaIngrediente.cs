namespace PizzeriaApp.Data.Entities;

public class RecetaIngrediente
{
    public int RecetaIngredienteId { get; set; }
    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;
    public int InsumoId { get; set; }
    public Insumo Insumo { get; set; } = null!;
    public decimal CantidadRequerida { get; set; }
}