using Microsoft.AspNetCore.Mvc;
using NetForum.Business.Services;
using NetForum.Data.Entities;
using NetForum.MVC.Models;
using NetForum.Repository.Repositories;
using NetForum.Repository.Interfaces;

namespace NetForum.MVC.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaController(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var categorias = _repository.ObtenerTodas();

            return View(categorias);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoriaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            Categoria categoria = new Categoria()
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                EstadoId = 1
            };

            _repository.Agregar(categoria);

            return RedirectToAction(nameof(Index));
        }
    }
}
