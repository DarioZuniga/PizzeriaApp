using System.ComponentModel.DataAnnotations;

namespace NetForum.MVC.Models
{
    public class CategoriaViewModel
    {
        [Required]
        public required string Nombre { get; set; }

        [Required]
        public required string Descripcion { get; set; }
    }
}
