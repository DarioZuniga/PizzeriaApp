namespace PizzeriaApp.Data.Entities;

public class PromocionPersonalizada
{
    public int PromocionPersonalizadaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    // Criterios
    public int MinimoCompras { get; set; } = 5;
    public int DiasVentana { get; set; } = 30;

    // Beneficio
    public decimal PorcentajeDescuento { get; set; } = 10;

    // Estado
    public bool Activa { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Estadísticas
    public int VecesAplicada { get; set; } = 0;
    public decimal MontoDescontado { get; set; } = 0;
}