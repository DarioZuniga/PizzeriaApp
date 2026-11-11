namespace PizzeriaApp.Data.Entities;

public class PuntoRecompensa
{
    public int PuntoRecompensaId { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public int Puntos { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string Motivo { get; set; } = string.Empty;
}