using System;

namespace NetForum.Data.Entities;

public class Post
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public int UsuarioId { get; set; }
}