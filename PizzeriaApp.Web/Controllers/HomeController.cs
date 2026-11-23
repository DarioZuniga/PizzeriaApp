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
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.Activo)
            .ToListAsync();

        // HU_16 C3: reseñas aprobadas para mostrar en el Home
        var resenasAprobadas = await _context.Evaluaciones
            .Include(e => e.Pedido)
                .ThenInclude(p => p.Usuario)
            .Where(e => e.Aprobada)
            .OrderByDescending(e => e.Fecha)
            .Take(6)
            .ToListAsync();

        ViewBag.Resenas = resenasAprobadas;

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