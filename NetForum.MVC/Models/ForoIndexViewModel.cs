using Microsoft.AspNetCore.Mvc.Rendering;

namespace NetForum.MVC.Models;

public class ForoIndexViewModel
{
    public IEnumerable<ForoTemaViewModel> Temas { get; set; }
        = Enumerable.Empty<ForoTemaViewModel>();

    public IEnumerable<SelectListItem> Categorias { get; set; }
        = Enumerable.Empty<SelectListItem>();

    public string? Buscar { get; set; }

    public int? CategoriaId { get; set; }
}