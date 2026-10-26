namespace PizzeriaApp.Data.Entities;

public class DetallePedidoIngrediente
{
    public int DetallePedidoIngredienteId { get; set; }
    public int DetallePedidoId { get; set; }
    public DetallePedido DetallePedido { get; set; } = null!;
    
    public int IngredienteId { get; set; }
    public Ingrediente Ingrediente { get; set; } = null!;
    
    public decimal PrecioExtra { get; set; }
}