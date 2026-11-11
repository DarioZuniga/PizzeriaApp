namespace PizzeriaApp.Data.Entities;

public class DetalleOrdenCompra
{
    public int DetalleOrdenCompraId { get; set; }
    public int OrdenCompraId { get; set; }
    public OrdenCompra OrdenCompra { get; set; } = null!;
    public int InsumoId { get; set; }
    public Insumo Insumo { get; set; } = null!;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}