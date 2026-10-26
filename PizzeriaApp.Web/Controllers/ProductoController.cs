using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.MVC.Controllers;

[Authorize(Roles = "Administrador")]
public class ProductoController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public ProductoController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var productos = _context.Productos.ToList();
        return View(productos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categorias = _context.Categorias.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Producto producto)
    {
        if (ModelState.IsValid)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categorias = _context.Categorias.ToList();
        return View(producto);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var producto = _context.Productos.Find(id);
        if (producto == null) return NotFound();
        ViewBag.Categorias = _context.Categorias.ToList();
        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Producto producto)
    {
        if (ModelState.IsValid)
        {
            _context.Productos.Update(producto);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categorias = _context.Categorias.ToList();
        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var producto = _context.Productos.Find(id);
        if (producto != null)
        {
            _context.Productos.Remove(producto);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Personalizar(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.ProductoId == id);
        
        if (producto == null) return NotFound();

        ViewBag.Ingredientes = await _context.Ingredientes.Where(i => i.Activo).ToListAsync();
        ViewBag.Tamanos = await _context.Tamanos.Where(t => t.Activo).ToListAsync();
        ViewBag.TiposMasa = await _context.TiposMasa.Where(t => t.Activo).ToListAsync();

        return View(producto);
    }
}