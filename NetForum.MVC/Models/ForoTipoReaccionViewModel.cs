namespace NetForum.MVC.Models;

public class ForoTipoReaccionViewModel
{
    public int TipoReaccionId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public bool SeleccionadaPorUsuario { get; set; }
}