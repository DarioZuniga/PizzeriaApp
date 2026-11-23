namespace PizzeriaApp.Data.Entities;

public class Evaluacion
{
    public int EvaluacionId { get; set; }
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; } = null!;

    public int? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int Calificacion { get; set; } // 1 al 10
    public string Comentario { get; set; } = string.Empty;
    public bool Aprobada { get; set; } = false;

    // HU_17: respuesta del admin
    public string? RespuestaAdmin { get; set; }
    public DateTime? FechaRespuesta { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;
}