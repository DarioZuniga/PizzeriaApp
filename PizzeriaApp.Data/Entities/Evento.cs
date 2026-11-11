namespace PizzeriaApp.Data.Entities;

public class Evento
{
    public int EventoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal PorcentajeDescuento { get; set; }
    public bool Activo { get; set; } = true;
}