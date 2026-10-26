namespace PizzeriaApp.Data.Entities;

public class Producto
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string ImagenUrl { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
}