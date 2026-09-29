using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFlow.Data.Entities;

public class TipoReaccion
{
    public int TipoReaccionId { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Reaccion> Reacciones { get; set; } = new List<Reaccion>();
}