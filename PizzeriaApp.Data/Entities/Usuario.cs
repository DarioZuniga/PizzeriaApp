namespace PizzeriaApp.Data.Entities;

public class Usuario
{
    public int UsuarioId { get; set; }

    public int RolId { get; set; }
    public Rol Rol { get; set; } = null!;

    public int EstadoId { get; set; }
    public Estado Estado { get; set; } = null!;

    public string NombreUsuario { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
}