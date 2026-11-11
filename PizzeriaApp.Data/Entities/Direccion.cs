namespace PizzeriaApp.Data.Entities;

public class Direccion
{
    public int DireccionId { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public string Alias { get; set; } = string.Empty; // Ej: Casa, Oficina
    public string Detalle { get; set; } = string.Empty; // Ej: 200m norte de la iglesia
    public bool Activa { get; set; } = true;
}