namespace PizzeriaApp.Data.Entities;

public class MovimientoInventario
{
    public int MovimientoInventarioId { get; set; }
    public int InsumoId { get; set; }
    public Insumo Insumo { get; set; } = null!;
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string TipoMovimiento { get; set; } = string.Empty; // Entrada, Salida, Merma, Ajuste
    public decimal Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string UsuarioResponsable { get; set; } = string.Empty;
}