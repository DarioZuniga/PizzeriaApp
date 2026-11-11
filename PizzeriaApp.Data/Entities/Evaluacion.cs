namespace PizzeriaApp.Data.Entities;

public class Evaluacion
{
    public int EvaluacionId { get; set; }
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; } = null!;
    public int Calificacion { get; set; } // 1 al 10
    public string Comentario { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;
}