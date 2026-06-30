using NetForum.Data.Entities;
using NetForum.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetForum.Business.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public List<Categoria> ObtenerCategorias()
        {
            return _repository.ObtenerTodas();
        }

        public void Guardar(Categoria categoria)
        {
            _repository.Agregar(categoria);
        }
    }
}