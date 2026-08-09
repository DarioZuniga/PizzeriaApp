using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetForum.MVC.Security;

namespace NetForum.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}