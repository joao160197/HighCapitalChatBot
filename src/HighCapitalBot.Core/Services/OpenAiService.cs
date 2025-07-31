using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HighCapitalBot.Core.Configuration;
using HighCapitalBot.Core.DTOs.AI;
using HighCapitalBot.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HighCapitalBot.Core.Services;

public class OpenAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly OpenAISettings _settings;
    private readonly ILogger<OpenAiService> _logger;
    private const string OpenAiBaseUrl = "https://api.openai.com/v1/chat/completions";

    public OpenAiService(HttpClient httpClient, IOptions<OpenAISettings> settings, ILogger<OpenAiService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
        
        // Configura o cliente HTTP
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GetResponseAsync(string message, string? context = null)
    {
        try
        {
            var messages = new List<OpenAiMessage>();
            
            // Adiciona o contexto se fornecido
            if (!string.IsNullOrWhiteSpace(context))
            {
                messages.Add(new OpenAiMessage 
                { 
                    Role = "system", 
                    Content = context 
                });
            }
            
            // Adiciona a mensagem do usuário
            messages.Add(new OpenAiMessage 
            { 
                Role = "user", 
                Content = message 
            });

            var request = new OpenAiRequest
            {
                Model = _settings.ModelName,
                Messages = messages,
                Temperature = _settings.Temperature,
                MaxTokens = _settings.MaxTokens
            };

            var response = await SendRequestAsync(request);
            return response?.Choices?.FirstOrDefault()?.Message?.Content ?? "Desculpe, não consegui processar sua mensagem.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter resposta da OpenAI");
            return "Desculpe, ocorreu um erro ao processar sua mensagem. Por favor, tente novamente mais tarde.";
        }
    }

    public async Task<string> GetResponseWithHistoryAsync(List<OpenAiMessage> messageHistory)
    {
        try
        {
            var request = new OpenAiRequest
            {
                Model = _settings.ModelName,
                Messages = messageHistory,
                Temperature = _settings.Temperature,
                MaxTokens = _settings.MaxTokens
            };

            var response = await SendRequestAsync(request);
            return response?.Choices?.FirstOrDefault()?.Message?.Content ?? "Desculpe, não consegui processar sua mensagem.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter resposta da OpenAI com histórico");
            return "Desculpe, ocorreu um erro ao processar sua mensagem. Por favor, tente novamente mais tarde.";
        }
    }

    private async Task<OpenAiResponse?> SendRequestAsync(OpenAiRequest request)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(OpenAiBaseUrl, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OpenAiResponse>(
            responseContent, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
