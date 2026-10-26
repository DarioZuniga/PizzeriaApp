namespace PizzeriaApp.Data.Entities;

public class TokenRecuperacion
{
    public int TokenRecuperacionId { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
    public DateTime FechaExpiracion { get; set; }
    public bool Usado { get; set; } = false;
}