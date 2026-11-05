using MiniCRM.Services.Interfaces;
using System.Text;
using System.Text.Json;

public class WahaService : IWahaService
{
    private readonly HttpClient _httpClient;
    private readonly string _wahaUrl = "http://localhost:3000/api/sendText";
    private readonly string _apiKey = "123456789";

    public WahaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task EnviarMensagemAsync(string telefone, string mensagem)
    {
        var numeroLimpo = new string(telefone.Where(char.IsDigit).ToArray());
        var chatId = $"{numeroLimpo}@c.us";

        var payload = new
        {
            session = "default",
            chatId = chatId,
            text = mensagem

            // linkPreview e reply_to são opcionais e podem ser omitidos
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        // Adiciona o cabeçalho de autenticação
        var request = new HttpRequestMessage(HttpMethod.Post, _wahaUrl)
        {
            Content = content
        };
        request.Headers.Add("X-Api-Key", _apiKey);

        try
        {
            // Envia a requisição e verifica se foi bem-sucedida
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                // Isso vai te mostrar a mensagem de erro exata do WAHA no console/logs
                throw new HttpRequestException(
                    $"WAHA API retornou {response.StatusCode}. Detalhes: {errorBody}");
            }
        }
        catch (Exception ex)
        {
            // Registre o erro (ILogger, Console, etc.)
            Console.WriteLine($"[WahaService] Falha ao enviar mensagem: {ex.Message}");
            throw;
        }
    }
}