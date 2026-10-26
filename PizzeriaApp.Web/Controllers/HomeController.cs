using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Data;
using PizzeriaApp.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaApp.MVC.Controllers;

public class HomeController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public HomeController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Cargamos los productos activos con sus categorías
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.Activo)
            .ToListAsync();

        return View(productos);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}