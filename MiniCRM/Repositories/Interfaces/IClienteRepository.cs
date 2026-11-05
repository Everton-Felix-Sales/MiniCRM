using MiniCRM.Models;

namespace MiniCRM.Repositories.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<Cliente> ObterComCampanhasAsync(Guid id);
        Task<(IEnumerable<Cliente> Clientes, int Total)> ObterFiltradoPaginadoAsync(string nome, int page, int pageSize);
    }
}
