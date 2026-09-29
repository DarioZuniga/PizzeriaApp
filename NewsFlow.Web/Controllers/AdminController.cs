using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsFlow.MVC.Security;

namespace NewsFlow.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}