using MiniCRM.Models;

namespace MiniCRM.Repositories.Interfaces
{
    public interface ICampanhaRepository : IRepository<Campanha>
    {
        Task<(IEnumerable<Campanha> Campanhas, int Total)> ObterFiltradoPaginadoAsync(
            string nomeCliente,
            DateTime? dataInicio,
            DateTime? dataFim,
            decimal? valorMin,
            decimal? valorMax,
            int page,
            int pageSize);
    }
}
