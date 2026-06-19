using NetForum.Data.Entities;
using System.Threading.Tasks;

namespace NetForum.Repository.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorEmailAsync(string email);
    Task InsertarUsuarioAsync(Usuario usuario);
}