namespace PizzeriaApp.Data.Entities;

public class OrdenCompra
{
    public int OrdenCompraId { get; set; }
    public int ProveedorId { get; set; }
    public Proveedor Proveedor { get; set; } = null!;
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "Pendiente"; // Pendiente, Recibida
    public decimal Total { get; set; }
    public ICollection<DetalleOrdenCompra> Detalles { get; set; } = new List<DetalleOrdenCompra>();
}