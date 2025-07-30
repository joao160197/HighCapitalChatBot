using HighCapitalBot.Core.DTOs;

namespace HighCapitalBot.Core.Interfaces;

public interface IChatService
{
    Task<ChatResponseDto> SendMessageAsync(int botId, string message);
    Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int botId);
    Task ClearChatHistoryAsync(int botId);
}
