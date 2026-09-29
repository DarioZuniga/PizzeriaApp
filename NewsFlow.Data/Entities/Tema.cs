using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFlow.Data.Entities;

public class Tema
{
    public int TemaId { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    public int EstadoId { get; set; }
    public Estado Estado { get; set; } = null!;

    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public ICollection<Respuesta> Respuestas { get; set; } = new List<Respuesta>();
    public ICollection<Reaccion> Reacciones { get; set; } = new List<Reaccion>();
    public ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
    public ICollection<TemaEtiqueta> TemaEtiquetas { get; set; } = new List<TemaEtiqueta>();
}