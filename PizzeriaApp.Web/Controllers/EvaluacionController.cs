using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;
using System.Security.Claims;

namespace PizzeriaApp.MVC.Controllers;

[Authorize]
public class EvaluacionController : Controller
{
    private readonly PizzeriaAppDbContext _context;
    public EvaluacionController(PizzeriaAppDbContext context) { _context = context; }

    [HttpGet]
    public IActionResult Crear(int pedidoId)
    {
        ViewBag.PedidoId = pedidoId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int pedidoId, int calificacion, string comentario)
    {
        var evaluacion = new Evaluacion
        {
            PedidoId = pedidoId,
            Calificacion = calificacion,
            Comentario = comentario
        };
        _context.Evaluaciones.Add(evaluacion);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = "¡Gracias por tu evaluación!";
        return RedirectToAction("Historial", "Pedido");
    }
}