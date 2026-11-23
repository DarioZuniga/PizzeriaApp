using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuarioController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public UsuarioController(PizzeriaAppDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        return View(usuarios);
    }

    [HttpPost]
    public async Task<IActionResult> CambiarRol(int usuarioId, int nuevoRolId)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario != null)
        {
            usuario.RolId = nuevoRolId;
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Rol actualizado correctamente.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int usuarioId)
    {
        var usuario = await _context.Usuarios.FindAsync(usuarioId);
        if (usuario != null && usuario.Correo != User.Identity?.Name) // No eliminarse a sí mismo
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Usuario eliminado.";
        }
        return RedirectToAction(nameof(Index));
    }
}