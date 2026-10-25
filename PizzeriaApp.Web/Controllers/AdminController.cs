using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.MVC.Security;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}