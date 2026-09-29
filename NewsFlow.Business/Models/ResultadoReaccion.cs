namespace NewsFlow.Business.Models;

public class ResultadoReaccion
{
    public bool Agregada { get; set; }

    public string Mensaje { get; set; } = string.Empty;

    public int TotalReacciones { get; set; }
}