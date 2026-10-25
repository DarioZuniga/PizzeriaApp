using System;
using System.Collections.Generic;
using System.Text;

namespace PizzeriaApp.Data.Entities;

public class Liga
{
    public int LigaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;

    public ICollection<Equipo> Equipos { get; set; } = new List<Equipo>();
}