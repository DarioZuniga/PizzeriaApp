using Microsoft.AspNetCore.Mvc.Rendering;

namespace PizzeriaApp.MVC.Models;
//SOLID: Single Responsibility Principle - esta clase solo tiene la responsabilidad de representar la vista del �ndice del foro
public class ForoIndexViewModel
{
    public IEnumerable<ForoTemaViewModel> Temas { get; set; }
        = Enumerable.Empty<ForoTemaViewModel>();
    public IEnumerable<SelectListItem> Categorias { get; set; }
        = Enumerable.Empty<SelectListItem>();
    public string? Buscar { get; set; }

    public int? CategoriaId { get; set; }
    public int? EtiquetaId { get; set; }
    public string? EtiquetaNombre { get; set; }
}