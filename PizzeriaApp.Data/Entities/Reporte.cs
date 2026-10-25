using System;
using System.Collections.Generic;
using System.Text;

namespace PizzeriaApp.Data.Entities;

public class Reporte
{
    public int ReporteId { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int TemaId { get; set; }
    public Tema Tema { get; set; } = null!;

    public int EstadoId { get; set; }
    public Estado Estado { get; set; } = null!;

    public string Motivo { get; set; } = string.Empty;
    public DateTime FechaReporte { get; set; } = DateTime.Now;
}