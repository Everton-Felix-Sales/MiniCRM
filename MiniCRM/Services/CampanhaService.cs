using MiniCRM.Models;
using MiniCRM.Repositories.Interfaces;
using MiniCRM.Services.Interfaces;

namespace MiniCRM.Services
{
    public class CampanhaService(ICampanhaRepository _campanhaRepository) : ICampanhaService
    {

        public async Task<IEnumerable<Campanha>> ObterTodosAsync()
        {
            return await _campanhaRepository.ObterTodosAsync();
        }

        public async Task<Campanha> ObterPorIdAsync(Guid id)
        {
            return await _campanhaRepository.ObterPorIdAsync(id);
        }

        public async Task AdicionarCampanhaAsync(Campanha Campanha)
        {
            // 1. Regra de negócio (ex: garantir que o Campanha seja salvo em maiúsculo)
            Campanha.Nome = Campanha.Nome.ToUpper();
            Campanha.Status = "Planejada";
            // 2. Salva no banco de dados usando o Repositório
            await _campanhaRepository.AdicionarAsync(Campanha);
        }

        public async Task AtualizarCampanhaAsync(Campanha Campanha)
        {
            await _campanhaRepository.AtualizarAsync(Campanha);
        }

        public async Task DeletarCampanhaAsync(Guid id)
        {
            await _campanhaRepository.DeletarAsync(id);
        }

        public async Task<(IEnumerable<Campanha> Campanhas, int Total)> ObterFiltradoPaginadoAsync(string nomeCliente, DateTime? dataInicio, DateTime? dataFim, decimal? valorMin, decimal? valorMax, int page, int pageSize)
        {
            return await _campanhaRepository.ObterFiltradoPaginadoAsync(nomeCliente, dataInicio, dataFim, valorMin, valorMax, page, pageSize);
        }

        public async Task AlternarStatusAsync(Guid id)
        {
            var campanha = await _campanhaRepository.ObterPorIdAsync(id);
            if (campanha == null) return;

            campanha.Ativo = !campanha.Ativo;
            campanha.DataAtualizacao = DateTime.Now;

            await _campanhaRepository.AtualizarAsync(campanha);
        }
    }
}
