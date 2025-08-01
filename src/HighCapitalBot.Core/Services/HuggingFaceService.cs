using System.Net.Http.Headers;
using System.Net.Http.Json;
using HighCapitalBot.Core.DTOs.AI;
using HighCapitalBot.Core.Interfaces;
using HighCapitalBot.Core.Models.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HighCapitalBot.Core.Services;

public class HuggingFaceService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly HuggingFaceSettings _settings;
    // O construtor agora aceita um HttpClient pré-configurado e as configurações.
    public HuggingFaceService(HttpClient httpClient, IOptions<HuggingFaceSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value; // Ainda precisamos das configurações para o nome do modelo.
    }

    public async Task<string> GetResponseAsync(string message, string? context = null)
    {
        var messages = new List<OpenAiMessage>();

        if (!string.IsNullOrEmpty(context))
        {
            messages.Add(new OpenAiMessage { Role = "system", Content = context });
        }

        messages.Add(new OpenAiMessage { Role = "user", Content = message });

        return await GetResponseWithHistoryAsync(messages);
    }

    public async Task<string> GetResponseWithHistoryAsync(List<OpenAiMessage> messages)
    {
        var (pastUserInputs, generatedResponses) = PrepareHistory(messages);

        var requestPayload = new
        {
            inputs = new
            {
                past_user_inputs = pastUserInputs,
                generated_responses = generatedResponses,
                text = messages.Last().Content
            }
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(_settings.ModelName, requestPayload);

            if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                return "O modelo está sendo carregado, por favor, aguarde e tente novamente em alguns instantes.";
            }

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var hfResponse = JsonConvert.DeserializeObject<HuggingFaceResponse>(responseString);

            return hfResponse?.GeneratedText ?? "Não foi possível obter uma resposta.";
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"\nException Caught!");
            Console.WriteLine($"Message: {e.Message}");
            return $"Erro ao contatar a API do Hugging Face: {e.Message}";
        }
    }

    private (List<string> pastUserInputs, List<string> generatedResponses) PrepareHistory(List<OpenAiMessage> messages)
    {
        var pastUserInputs = new List<string>();
        var generatedResponses = new List<string>();

        var historyMessages = messages.Where(m => m.Role != "system").SkipLast(1).ToList();

        for (int i = 0; i < historyMessages.Count; i += 2)
        {
            if (i + 1 < historyMessages.Count && historyMessages[i].Role == "user" && historyMessages[i + 1].Role == "assistant")
            {
                pastUserInputs.Add(historyMessages[i].Content);
                generatedResponses.Add(historyMessages[i + 1].Content);
            }
        }

        return (pastUserInputs, generatedResponses);
    }
}