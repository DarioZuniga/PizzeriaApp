using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.MVC.Models;
using PizzeriaApp.MVC.Security;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = RolesSistema.Administrador)]
public class ReporteController : Controller
{
    // SOLID: Dependency Inversion Principle - el controller depende de la abstracci�n
    private readonly IReporteService _reporteService;

    public ReporteController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reportes = await _reporteService.ObtenerReportesAsync();
        var estados = await _reporteService.ObtenerEstadosAsync();

        ViewBag.Estados = estados
            .Select(e => new SelectListItem
            {
                Value = e.EstadoId.ToString(),
                Text = e.Nombre
            })
            .ToList();

        return View(reportes);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var reporte = await _reporteService.ObtenerPorIdAsync(id);

        if (reporte is null)
        {
            return NotFound(new
            {
                success = false,
                message = "El reporte no existe."
            });
        }

        return Json(new
        {
            reporteId = reporte.ReporteId,
            estadoId = reporte.EstadoId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstadoAjax(
        ReporteViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var errores = ModelState
                .Where(m => m.Value?.Errors.Count > 0)
                .SelectMany(m => m.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new
            {
                success = false,
                message = errores.FirstOrDefault()
                          ?? "Los datos enviados no son v�lidos."
            });
        }

        try
        {
            await _reporteService.CambiarEstadoAsync(
                viewModel.ReporteId,
                viewModel.EstadoId);

            return Json(new
            {
                success = true,
                message = "El estado del reporte se actualiz� correctamente."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAjax(int id)
    {
        var reporte = await _reporteService.ObtenerPorIdAsync(id);

        if (reporte is null)
        {
            return NotFound(new
            {
                success = false,
                message = "El reporte no existe."
            });
        }

        await _reporteService.EliminarAsync(id);

        return Json(new
        {
            success = true,
            message = "El reporte se elimin� correctamente."
        });
    }
}