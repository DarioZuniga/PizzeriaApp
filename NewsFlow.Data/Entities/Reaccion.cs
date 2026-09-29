using System;
using System.Collections.Generic;
using System.Text;

namespace NewsFlow.Data.Entities;

public class Reaccion
{
    public int ReaccionId { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int TemaId { get; set; }
    public Tema Tema { get; set; } = null!;

    public int TipoReaccionId { get; set; }
    public TipoReaccion TipoReaccion { get; set; } = null!;
}
