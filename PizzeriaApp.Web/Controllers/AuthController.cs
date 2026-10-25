using Microsoft.AspNetCore.Mvc;

namespace PizzeriaApp.MVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
