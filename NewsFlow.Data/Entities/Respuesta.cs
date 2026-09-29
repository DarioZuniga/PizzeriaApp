using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFlow.Data.Entities;

public class Respuesta
{
    public int RespuestaId { get; set; }

    public int TemaId { get; set; }
    public Tema Tema { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int EstadoId { get; set; }
    public Estado Estado { get; set; } = null!;

    public string Contenido { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}