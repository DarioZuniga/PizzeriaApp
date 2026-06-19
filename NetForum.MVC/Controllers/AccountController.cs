using Microsoft.AspNetCore.Mvc;
using NetForum.MVC.Models; // <-- Asegúrate de que esta línea tenga MVC en mayúsculas

namespace NetForum.MVC.Controllers; // <-- Y esta también

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }
}