using Microsoft.AspNetCore.Mvc;

namespace NetForum.MVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
