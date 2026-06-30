using NetForum.Data.Entities;
using System.Threading.Tasks;

namespace NetForum.Repository.Interfaces;

// DP: Repository Pattern
// SOLID: Interface Segregation Principle - contrato específico para Categoria.
public interface ICategoriaRepository : IGenericRepository<Categoria>
{
    
        List<Categoria> ObtenerTodas();

        void Agregar(Categoria categoria);
   
}