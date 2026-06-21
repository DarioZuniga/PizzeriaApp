using System;

namespace NetForum.Data.Entities;

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

    public ICollection<Tema> Temas { get; set; } = new List<Tema>();
    public ICollection<Respuesta> Respuestas { get; set; } = new List<Respuesta>();
    public ICollection<Reaccion> Reacciones { get; set; } = new List<Reaccion>();
    public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
    public ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
}

