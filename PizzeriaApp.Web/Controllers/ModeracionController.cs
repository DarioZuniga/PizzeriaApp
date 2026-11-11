using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class ModeracionController : Controller
{
    private readonly PizzeriaAppDbContext _context;
    public ModeracionController(PizzeriaAppDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var evaluaciones = await _context.Evaluaciones
            .Include(e => e.Pedido)
            .ThenInclude(p => p.Usuario)
            .OrderByDescending(e => e.Fecha)
            .ToListAsync();
        return View(evaluaciones);
    }
}