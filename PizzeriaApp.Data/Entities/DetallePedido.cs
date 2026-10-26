namespace PizzeriaApp.Data.Entities;

public class DetallePedido
{
    public int DetallePedidoId { get; set; }
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; } = null!;
    
    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;
    
    public int Cantidad { get; set; } = 1;
    public decimal PrecioUnitario { get; set; }
    
    public int? TamanoId { get; set; }
    public Tamano? Tamano { get; set; }
    
    public int? TipoMasaId { get; set; }
    public TipoMasa? TipoMasa { get; set; }
    
    public ICollection<DetallePedidoIngrediente> Ingredientes { get; set; } = new List<DetallePedidoIngrediente>();
}