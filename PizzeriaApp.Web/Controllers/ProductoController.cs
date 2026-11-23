using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.MVC.Controllers;

public class ProductoController : Controller
{
    private readonly PizzeriaAppDbContext _context;

    public ProductoController(PizzeriaAppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Index()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return View(productos);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Create()
    {
        await CargarCategorias();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Producto producto)
    {
        ModelState.Remove(nameof(producto.Categoria));

        if (!await _context.Categorias.AnyAsync(c =>
            c.CategoriaId == producto.CategoriaId && c.Activo))
        {
            ModelState.AddModelError(nameof(producto.CategoriaId),
                "Seleccione una categoría válida y activa.");
        }

        if (ModelState.IsValid)
        {
            producto.Activo = true;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Producto agregado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategorias(producto.CategoriaId);
        return View(producto);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Edit(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
            return NotFound();

        await CargarCategorias(producto.CategoriaId);
        return View(producto);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Producto producto)
    {
        if (id != producto.ProductoId)
            return BadRequest();

        ModelState.Remove(nameof(producto.Categoria));

        var existente = await _context.Productos.FindAsync(id);

        if (existente == null)
            return NotFound();

        if (!await _context.Categorias.AnyAsync(c =>
            c.CategoriaId == producto.CategoriaId && c.Activo))
        {
            ModelState.AddModelError(nameof(producto.CategoriaId),
                "Seleccione una categoría válida y activa.");
        }

        if (ModelState.IsValid)
        {
            existente.Nombre = producto.Nombre;
            existente.Descripcion = producto.Descripcion;
            existente.Precio = producto.Precio;
            existente.CategoriaId = producto.CategoriaId;
            existente.ImagenUrl = producto.ImagenUrl;
            existente.Activo = producto.Activo;

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Producto actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategorias(producto.CategoriaId);
        return View(producto);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
        {
            TempData["Mensaje"] = "El producto no existe.";
            return RedirectToAction(nameof(Index));
        }

        var tienePedidos = await _context.Set<DetallePedido>()
            .AnyAsync(d => d.ProductoId == id);

        var tieneRecetas = await _context.Set<RecetaIngrediente>()
            .AnyAsync(r => r.ProductoId == id);

        if (tienePedidos || tieneRecetas)
        {
            producto.Activo = false;
            await _context.SaveChangesAsync();

            TempData["Mensaje"] =
                "Producto desactivado para conservar el historial de pedidos y recetas.";
        }
        else
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Producto eliminado correctamente.";
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Personalizar(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.ProductoId == id && p.Activo);

        if (producto == null)
            return NotFound();

        ViewBag.Ingredientes = await _context.Ingredientes
            .Where(i => i.Activo).ToListAsync();

        ViewBag.Tamanos = await _context.Tamanos
            .Where(t => t.Activo).ToListAsync();

        ViewBag.TiposMasa = await _context.TiposMasa
            .Where(t => t.Activo).ToListAsync();

        return View(producto);
    }

    private async Task CargarCategorias(int? categoriaSeleccionada = null)
    {
        ViewBag.Categorias = await _context.Categorias
            .Where(c => c.Activo)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        ViewBag.CategoriaSeleccionada = categoriaSeleccionada;
    }
}
