using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class EventoController : Controller
{
    private readonly PizzeriaAppDbContext _context;
    public EventoController(PizzeriaAppDbContext context) { _context = context; }

    public async Task<IActionResult> Index() => View(await _context.Eventos.ToListAsync());

    [HttpGet]
    public IActionResult Crear() => View();

    [HttpPost]
    public async Task<IActionResult> Crear(Evento evento)
    {
        if (ModelState.IsValid)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(evento);
    }
}