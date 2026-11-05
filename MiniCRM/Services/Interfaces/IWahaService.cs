namespace MiniCRM.Services.Interfaces
{
    public interface IWahaService
    {
        Task EnviarMensagemAsync(string telefone, string mensagem);
    }
}
