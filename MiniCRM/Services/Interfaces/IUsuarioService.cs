using MiniCRM.Models;

namespace MiniCRM.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> AutenticarAsync(string email, string senha);
        Task CriarUsuarioAsync(Usuario usuario, string senhaPlana);
    }
}