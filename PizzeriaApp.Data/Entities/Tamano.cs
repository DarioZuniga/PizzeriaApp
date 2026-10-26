namespace PizzeriaApp.Data.Entities;

public class Tamano
{
    public int TamanoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal FactorPrecio { get; set; } = 1.0m;
    public bool Activo { get; set; } = true;
}