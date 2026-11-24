using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Data.Entities;
using PizzeriaApp.Data;
using PizzeriaApp.MVC.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PizzeriaApp.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly PizzeriaAppDbContext _context;

    public AccountController(
        IAuthService authService,
        IEmailService emailService,
        PizzeriaAppDbContext context)
    {
        _authService = authService;
        _emailService = emailService;
        _context = context;
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
            ModelState.AddModelError(string.Empty, "Correo o contraseÃ±a incorrectos.");
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
            RolId = 2 // Cliente
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
            ModelState.AddModelError("", "Ingrese un correo vÃ¡lido.");
            return View();
        }

        var usuario = await _authService.ObtenerUsuarioPorCorreoAsync(correo);
        if (usuario != null)
        {
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var enlaceRecuperacion = await _authService.GuardarTokenRecuperacionAsync(usuario.UsuarioId, token);
            
            string urlCompleta = $"{Request.Scheme}://{Request.Host}{enlaceRecuperacion}";
            string cuerpo = $"<h3>RecuperaciÃ³n de ContraseÃ±a</h3><p>Haz clic en el siguiente enlace:</p><a href='{urlCompleta}'>Restablecer ContraseÃ±a</a>";
            
            try {
                await _emailService.EnviarCorreoAsync(correo, "RecuperaciÃ³n de ContraseÃ±a - Las Perlas", cuerpo);
                TempData["Mensaje"] = "Se ha enviado un enlace a tu correo.";
            } catch {
                TempData["Mensaje"] = "Error al enviar el correo. Verifica la configuraciÃ³n SMTP.";
            }
            return RedirectToAction("Login");
        }
        
        TempData["Mensaje"] = "Si el correo existe, recibirÃ¡s un enlace.";
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
            ModelState.AddModelError("", "La contraseÃ±a debe tener al menos 6 caracteres.");
            ViewBag.Token = token;
            return View();
        }

        var exito = await _authService.CambiarPasswordConTokenAsync(token, nuevaPassword);
        if (!exito)
        {
            ModelState.AddModelError("", "El token es invÃ¡lido o ha expirado.");
            ViewBag.Token = token;
            return View();
        }

        TempData["Mensaje"] = "ContraseÃ±a cambiada exitosamente. Inicie sesiÃ³n.";
        return RedirectToAction("Login");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("CookieAuth");
        return RedirectToAction("Login");
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int usuarioId))
            return RedirectToAction(nameof(Login));

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Estado)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario == null)
            return NotFound();

        return View(CrearPerfilViewModel(usuario));
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(PerfilViewModel model)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int usuarioId))
            return RedirectToAction(nameof(Login));

        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .Include(u => u.Estado)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            CargarDatosPerfil(model, usuario);
            return View(model);
        }

        var correo = model.Correo.Trim();

        var correoExiste = await _context.Usuarios.AnyAsync(u =>
            u.UsuarioId != usuarioId &&
            u.Correo.ToLower() == correo.ToLower());

        if (correoExiste)
        {
            ModelState.AddModelError(nameof(model.Correo),
                "Este correo ya está registrado en otra cuenta.");

            CargarDatosPerfil(model, usuario);
            return View(model);
        }

        usuario.NombreUsuario = model.NombreUsuario.Trim();
        usuario.Correo = correo;

        await _context.SaveChangesAsync();

        // Actualizar los datos de la sesión autenticada
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
        };

        var identity = new ClaimsIdentity(claims, "CookieAuth");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("CookieAuth", principal);

        TempData["Mensaje"] = "Tu perfil se actualizó correctamente.";

        return RedirectToAction(nameof(Profile));
    }

    private static PerfilViewModel CrearPerfilViewModel(Usuario usuario)
    {
        var model = new PerfilViewModel
        {
            NombreUsuario = usuario.NombreUsuario,
            Correo = usuario.Correo
        };

        CargarDatosPerfil(model, usuario);
        return model;
    }

    private static void CargarDatosPerfil(PerfilViewModel model, Usuario usuario)
    {
        model.Rol = usuario.Rol?.Nombre ?? "No disponible";
        model.Estado = usuario.Estado?.Nombre ?? "No disponible";
        model.FechaRegistro = usuario.FechaRegistro;
    }
    [HttpGet]
    public IActionResult AccessDenied() => View();
}
