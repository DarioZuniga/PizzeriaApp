using System;
using System.Collections.Generic;
using System.Text;

namespace NetForum.Data.Entities;

public class Estado
{
    public int EstadoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
    public ICollection<Tema> Temas { get; set; } = new List<Tema>();
    public ICollection<Respuesta> Respuestas { get; set; } = new List<Respuesta>();
    public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
}