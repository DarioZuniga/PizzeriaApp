namespace PizzeriaApp.Data.Entities;

public class TipoMasa
{
    public int TipoMasaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioExtra { get; set; }
    public bool Activo { get; set; } = true;
}