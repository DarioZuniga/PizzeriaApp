using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsFlow.Business.Interfaces;
using NewsFlow.Data.Entities;
using NewsFlow.MVC.Models;
using System.Security.Claims;

namespace NewsFlow.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService; // AUTHENTICATION SERVICE
    private readonly IPerfilService _perfilService; // PROFILE SERVICE


    public AccountController(
     IAuthService authService,
     IPerfilService perfilService)
    {
        _authService = authService;
        _perfilService = perfilService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var usuario = await _authService.LoginAsync(model.Correo, model.Password);

        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(model);
        }

        var claims = new List<Claim>
{
    new Claim(
        ClaimTypes.NameIdentifier,
        usuario.UsuarioId.ToString()),

    new Claim(
        ClaimTypes.Name,
        usuario.NombreUsuario),

    new Claim(
        ClaimTypes.Email,
        usuario.Correo),

    new Claim(
        ClaimTypes.Role,
        usuario.Rol.Nombre)
};

        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync("CookieAuth", principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var usuario = new Usuario
        {
            NombreUsuario = model.NombreUsuario,
            Correo = model.Email,
            EstadoId = 1,
            RolId = 3
        };

        try
        {
            await _authService.RegistrarUsuarioAsync(usuario, model.Password);
            return RedirectToAction("Login");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("Email", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    [Authorize]
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var usuarioIdTexto = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdTexto, out var usuarioId))
        {
            return RedirectToAction(nameof(Login));
        }

        var usuario = await _perfilService.ObtenerPerfilAsync(usuarioId);

        if (usuario is null)
        {
            return NotFound();
        }

        var model = new PerfilUsuarioViewModel
        {
            UsuarioId = usuario.UsuarioId,
            NombreUsuario = usuario.NombreUsuario,
            Correo = usuario.Correo,
            Rol = usuario.Rol.Nombre,
            Estado = usuario.Estado.Nombre,
            FechaRegistro = usuario.FechaRegistro,

            TotalTemas =
                await _perfilService.ContarTemasAsync(usuarioId),

            TotalRespuestas =
                await _perfilService.ContarRespuestasAsync(usuarioId),

            TotalReacciones =
                await _perfilService.ContarReaccionesAsync(usuarioId),

            Temas = usuario.Temas
                .OrderByDescending(t => t.FechaCreacion)
                .Select(t => new PerfilTemaViewModel
                {
                    TemaId = t.TemaId,
                    Titulo = t.Titulo,
                    Categoria = t.Categoria.Nombre,
                    Estado = t.Estado.Nombre,
                    FechaCreacion = t.FechaCreacion,
                    CantidadRespuestas = t.Respuestas.Count,
                    CantidadReacciones = t.Reacciones.Count
                })
                .ToList()
        };

        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
