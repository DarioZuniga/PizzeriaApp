using System;
using System.Collections.Generic;
using System.Text;

namespace NetForum.Data.Entities;

public class Notificacion
{
    public int NotificacionId { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public string Mensaje { get; set; } = string.Empty;
    public bool Leida { get; set; } = false;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}