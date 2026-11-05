using Microsoft.EntityFrameworkCore;
using MiniCRM.Data;
using MiniCRM.Models;
using MiniCRM.Repositories.Interfaces;

namespace MiniCRM.Repositories
{
    public class CampanhaRepository : BaseRepository<Campanha>, ICampanhaRepository
    {
        public CampanhaRepository(AppDbContext context) : base(context) { }

        public async Task<(IEnumerable<Campanha> Campanhas, int Total)> ObterFiltradoPaginadoAsync(
             string nomeCliente,
             DateTime? dataInicio,
             DateTime? dataFim,
             decimal? valorMin,
             decimal? valorMax,
             int page,
             int pageSize)
        {
            var query = _dbSet.Include(c => c.Cliente).AsQueryable();

            // Filtro por Nome do Cliente
            if (!string.IsNullOrWhiteSpace(nomeCliente))
            {
                var nomeLimpo = nomeCliente.Trim().ToLower();
                query = query.Where(c => c.Cliente != null && c.Cliente.Nome.ToLower().Contains(nomeLimpo));
            }

            // Filtro por Data Inicial (A partir de 00:00:00)
            if (dataInicio.HasValue)
            {
                query = query.Where(c => c.DataInicio >= dataInicio.Value.Date);
            }

            // Filtro por Data Final (Até 23:59:59)
            if (dataFim.HasValue)
            {
                var dataFimLimite = dataFim.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(c => c.DataFim <= dataFimLimite);
            }

            // Filtro por Faixa de Orçamento
            if (valorMin.HasValue && valorMin.Value > 0)
            {
                query = query.Where(c => c.Orcamento >= valorMin.Value);
            }

            if (valorMax.HasValue && valorMax.Value > 0)
            {
                query = query.Where(c => c.Orcamento <= valorMax.Value);
            }

            int total = await query.CountAsync();

            // Paginação com cálculo de Skip seguro
            int pageIndex = page < 1 ? 1 : page;
            var campanhas = await query.OrderByDescending(c => c.DataCriacao)
                                       .Skip((pageIndex - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            return (campanhas, total);
        }

    }
}
