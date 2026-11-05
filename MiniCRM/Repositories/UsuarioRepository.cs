using Microsoft.EntityFrameworkCore;
using MiniCRM.Data;
using MiniCRM.Models;
using MiniCRM.Repositories.Interfaces;

namespace MiniCRM.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}