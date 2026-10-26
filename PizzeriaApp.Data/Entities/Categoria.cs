namespace PizzeriaApp.Data.Entities;

public class Categoria
{
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}