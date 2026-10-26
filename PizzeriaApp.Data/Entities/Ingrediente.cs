namespace PizzeriaApp.Data.Entities;

public class Ingrediente
{
    public int IngredienteId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal PrecioExtra { get; set; }
    public bool Activo { get; set; } = true;
}