using Microsoft.EntityFrameworkCore;
using MiniCRM.Data;
using MiniCRM.Models;
using MiniCRM.Repositories.Interfaces;

namespace MiniCRM.Repositories
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AppDbContext context) : base(context) { }

        public async Task<Cliente> ObterComCampanhasAsync(Guid id)
        {
            return await _dbSet.Include(c => c.Campanhas).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<(IEnumerable<Cliente> Clientes, int Total)> ObterFiltradoPaginadoAsync(string nome, int page, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(c => c.Nome.Contains(nome));

            int total = await query.CountAsync();
            var clientes = await query.Include(c => c.Campanhas)
                                      .OrderBy(c => c.Nome)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();
            return (clientes, total);
        }
    }
}
