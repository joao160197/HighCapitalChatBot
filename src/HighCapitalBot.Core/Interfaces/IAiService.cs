using HighCapitalBot.Core.DTOs.AI;

namespace HighCapitalBot.Core.Interfaces;

public interface IAiService
{
    Task<string> GetResponseAsync(string message, string? context = null);
    Task<string> GetResponseWithHistoryAsync(List<OpenAiMessage> messageHistory);
}
