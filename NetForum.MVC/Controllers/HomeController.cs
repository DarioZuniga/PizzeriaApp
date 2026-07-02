using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetForum.Business.Interfaces;
using NetForum.MVC.Models;

namespace NetForum.MVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        // SOLID: Dependency Inversion Principle
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            NombreUsuario = User.Identity?.Name ?? "Futbolero",
            TotalUsuarios = await _dashboardService.ContarUsuariosAsync(),
            TotalTemas = await _dashboardService.ContarTemasAsync(),
            TotalRespuestas = await _dashboardService.ContarRespuestasAsync(),
            TotalEquipos = await _dashboardService.ContarEquiposAsync(),
            TotalLigas = await _dashboardService.ContarLigasAsync()
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}