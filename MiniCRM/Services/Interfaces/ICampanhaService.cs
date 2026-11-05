using MiniCRM.Models;

namespace MiniCRM.Services.Interfaces
{
    public interface ICampanhaService
    {
        Task<IEnumerable<Campanha>> ObterTodosAsync();
        Task<Campanha> ObterPorIdAsync(Guid id);
        Task AdicionarCampanhaAsync(Campanha Campanha);
        Task AtualizarCampanhaAsync(Campanha Campanha);
        Task DeletarCampanhaAsync(Guid id);
        Task AlternarStatusAsync(Guid id);
        Task<(IEnumerable<Campanha> Campanhas, int Total)> ObterFiltradoPaginadoAsync(
            string nomeCliente, DateTime? dataInicio, DateTime? dataFim,
            decimal? valorMin, decimal? valorMax, int page, int pageSize);
    }
}
