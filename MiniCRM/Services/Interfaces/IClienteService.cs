using MiniCRM.Models;

namespace MiniCRM.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObterTodosAsync();
        Task<Cliente> ObterPorIdAsync(Guid id);
        Task AdicionarClienteAsync(Cliente cliente);
        Task AtualizarClienteAsync(Cliente cliente);
        Task DeletarClienteAsync(Guid id);
        Task AlternarStatusAsync(Guid id);
        Task<(IEnumerable<Cliente> Clientes, int Total)> ObterFiltradoPaginadoAsync(string nome, int page, int pageSize);
    }
}
