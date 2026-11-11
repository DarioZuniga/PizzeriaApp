namespace PizzeriaApp.Data.Entities;

public class ConfiguracionReserva
{
    public int ConfiguracionReservaId { get; set; }
    public int LimiteReservasPorDia { get; set; } = 20;
    public TimeSpan HoraApertura { get; set; } = new TimeSpan(11, 0, 0);
    public TimeSpan HoraCierre { get; set; } = new TimeSpan(22, 0, 0);
    public bool Activo { get; set; } = true;
}