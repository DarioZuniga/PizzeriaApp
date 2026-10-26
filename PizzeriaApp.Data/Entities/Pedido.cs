namespace PizzeriaApp.Data.Entities;

public class Pedido
{
    public int PedidoId { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public DateTime Fecha { get; set; } = DateTime.Now;
    public decimal Subtotal { get; set; }
    public decimal Impuestos { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = "Pendiente"; // Pendiente, Pagado, EnPreparacion, EnCamino, Entregado, Cancelado
    public string MetodoPago { get; set; } = string.Empty; // Tarjeta, SINPE, ContraEntrega
    
    public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
}