using System;
using System.Collections.Generic;
using System.Text;

namespace PizzeriaApp.Data.Entities;

public class Categoria
{
    public int CategoriaId { get; set; }

    public int EstadoId { get; set; }
    public Estado Estado { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public ICollection<Tema> Temas { get; set; } = new List<Tema>();
}