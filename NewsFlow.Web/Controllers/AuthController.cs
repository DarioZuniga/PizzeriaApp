using Microsoft.AspNetCore.Mvc;

namespace NewsFlow.MVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
