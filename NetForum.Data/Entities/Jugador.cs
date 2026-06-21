using System;
using System.Collections.Generic;
using System.Text;

namespace NetForum.Data.Entities;

public class Jugador
{
    public int JugadorId { get; set; }

    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;
    public string Posicion { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = string.Empty;
}