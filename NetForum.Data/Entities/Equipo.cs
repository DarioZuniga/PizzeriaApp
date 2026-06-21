using System;
using System.Collections.Generic;
using System.Text;

namespace NetForum.Data.Entities;

public class Equipo
{
    public int EquipoId { get; set; }

    public int LigaId { get; set; }
    public Liga Liga { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public int Fundacion { get; set; }

    public ICollection<Jugador> Jugadores { get; set; } = new List<Jugador>();
}