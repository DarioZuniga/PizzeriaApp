using Microsoft.AspNetCore.Mvc;

namespace NetForum.MVC.Controllers
{
    public class CategoriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
