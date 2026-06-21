using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using NetForum.Data;
using NetForum.Data.Entities;
using NetForum.MVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace NetForum.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly NetForumDbContext _context;

        public AccountController(NetForumDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // VISTA DE LOGIN (GET)
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // ==========================================
        // PROCESAR LOGIN (POST)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == model.Email);

            if (usuario == null || usuario.PasswordHash != model.Password)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.RolId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // VISTA DE REGISTRO (GET)
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // ==========================================
        // PROCESAR REGISTRO (POST)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existeCorreo = await _context.Usuarios.AnyAsync(u => u.Correo == model.Email);
            if (existeCorreo)
            {
                ModelState.AddModelError("Email", "Este correo electrónico ya está registrado.");
                return View(model);
            }

            var nuevoUsuario = new NetForum.Data.Entities.Usuario
            {
                NombreUsuario = model.NombreUsuario,
                Correo = model.Email,
                PasswordHash = model.Password,
                EstadoId = 1,
                RolId = 3,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        // VISTA DE PERFIL (GET) - CORREGIDA SIN INCLUDE

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var emailUsuario = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailUsuario))
            {
                return RedirectToAction("Login");
            }

            
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == emailUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // CERRAR SESIÓN (LOGOUT)

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}