using MiniCRM.Models;
using MiniCRM.Repositories.Interfaces;
using MiniCRM.Services.Interfaces;

namespace MiniCRM.Services
{
    public class ClienteService(IClienteRepository _clienteRepository) : IClienteService
    {
        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
        {
            return await _clienteRepository.ObterTodosAsync();
        }

        public async Task<Cliente> ObterPorIdAsync(Guid id)
        {
            return await _clienteRepository.ObterPorIdAsync(id);
        }

        public async Task AdicionarClienteAsync(Cliente cliente)
        {
            cliente.Nome = cliente.Nome.ToUpper();

            await _clienteRepository.AdicionarAsync(cliente);
        }

        public async Task AtualizarClienteAsync(Cliente cliente)
        {
            await _clienteRepository.AtualizarAsync(cliente);
        }

        public async Task DeletarClienteAsync(Guid id)
        {
            await _clienteRepository.DeletarAsync(id);
        }
        public async Task<(IEnumerable<Cliente> Clientes, int Total)> ObterFiltradoPaginadoAsync(string nome, int page, int pageSize)
        {
            return await _clienteRepository.ObterFiltradoPaginadoAsync(nome, page, pageSize);
        }

        public async Task AlternarStatusAsync(Guid id)
        {
            // Procura o cliente juntamente com as suas campanhas
            var cliente = await _clienteRepository.ObterComCampanhasAsync(id);
            if (cliente == null) return;

            // Inverte o status atual do cliente (se estava Ativo vira Inativo, se estava Inativo vira Ativo)
            cliente.Ativo = !cliente.Ativo;
            cliente.DataAtualizacao = DateTime.Now;

            //Sincroniza o status de todas as campanhas associadas ao novo status do cliente
            if (cliente.Campanhas != null && cliente.Campanhas.Any())
            {
                foreach (var campanha in cliente.Campanhas)
                {
                    // Se o status da campanha for diferente do novo status do cliente, atualiza
                    if (campanha.Ativo != cliente.Ativo)
                    {
                        campanha.Ativo = cliente.Ativo;
                        campanha.DataAtualizacao = DateTime.Now;
                    }
                }
            }

            await _clienteRepository.AtualizarAsync(cliente);
        }
    }
}
