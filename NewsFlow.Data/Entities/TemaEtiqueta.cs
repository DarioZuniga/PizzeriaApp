namespace NewsFlow.Data.Entities;

public class TemaEtiqueta
{
    public int TemaEtiquetaId { get; set; }

    public int TemaId { get; set; }
    public Tema Tema { get; set; } = null!;

    public int EtiquetaId { get; set; }
    public Etiqueta Etiqueta { get; set; } = null!;
}