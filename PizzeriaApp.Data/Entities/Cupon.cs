namespace PizzeriaApp.Data.Entities;

public class Cupon
{
    public int CuponId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public decimal PorcentajeDescuento { get; set; } // Ej: 10 para 10%
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool Activo { get; set; } = true;
    public int UsosMaximos { get; set; } = 100;
    public int UsosActuales { get; set; } = 0;
}