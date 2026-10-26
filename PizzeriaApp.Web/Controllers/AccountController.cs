using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.MVC.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PizzeriaApp.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var usuario = await _authService.LoginAsync(model.Correo, model.Password);
        if (usuario is null)
        {
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(claimsIdentity);
        await HttpContext.SignInAsync("CookieAuth", principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var usuario = new Usuario
        {
            NombreUsuario = model.NombreUsuario,
            Correo = model.Email,
            EstadoId = 1,
            RolId = 2
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
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
        {
            ModelState.AddModelError("", "Ingrese un correo válido.");
            return View();
        }

        // Buscamos si el usuario existe
        var usuario = await _authService.ObtenerUsuarioPorCorreoAsync(correo);
        if (usuario != null)
        {
            // Generar token
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

            // Guardar token en BD (esto lo manejaremos en el AuthService)
            await _authService.GuardarTokenRecuperacionAsync(usuario.UsuarioId, token);

            // TODO: Enviar correo con MailKit. Por ahora, lo mostramos en consola.
            Console.WriteLine($"=== TOKEN DE RECUPERACIÓN PARA {correo}: {token} ===");
        }

        // Siempre mostramos el mismo mensaje (seguridad) para no revelar si el correo existe
        TempData["Mensaje"] = "Si el correo existe, se ha enviado un enlace de recuperación.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return RedirectToAction("Login");

        ViewBag.Token = token;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(string token, string nuevaPassword)
    {
        if (string.IsNullOrWhiteSpace(nuevaPassword) || nuevaPassword.Length < 6)
        {
            ModelState.AddModelError("", "La contraseña debe tener al menos 6 caracteres.");
            ViewBag.Token = token;
            return View();
        }

        var exito = await _authService.CambiarPasswordConTokenAsync(token, nuevaPassword);
        if (!exito)
        {
            ModelState.AddModelError("", "El token es inválido o ha expirado.");
            ViewBag.Token = token;
            return View();
        }

        TempData["Mensaje"] = "Contraseña cambiada exitosamente. Inicie sesión.";
        return RedirectToAction("Login");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();
}