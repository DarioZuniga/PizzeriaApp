using System.Threading.Tasks;

namespace NetForum.Business.Services;

public class AuthService
{
    public async Task<bool> ValidarLoginAsync(string email, string password)
    {
        // Cascarón estructural para el Avance 1
        return await Task.FromResult(false);
    }
}