namespace MiniCRM.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<T> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(T entidade);
        Task AtualizarAsync(T entidade);
        Task DeletarAsync(Guid id);
    }
}
