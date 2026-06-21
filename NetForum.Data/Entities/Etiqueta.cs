using System;
using System.Collections.Generic;
using System.Text;

namespace NetForum.Data.Entities;

public class Etiqueta
{
    public int EtiquetaId { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<TemaEtiqueta> TemaEtiquetas { get; set; } = new List<TemaEtiqueta>();
}