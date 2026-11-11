namespace PizzeriaApp.Data.Entities;

public class Reserva
{
    public int ReservaId { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public DateTime FechaHora { get; set; }
    public int CantidadPersonas { get; set; }
    public string Estado { get; set; } = "Pendiente"; // Pendiente, Confirmada, Cancelada
    public string Notas { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}