using HighCapitalBot.Core.DTOs;

namespace HighCapitalBot.Core.Interfaces;

public interface IChatService
{
    Task<ChatResponseDto> SendMessageAsync(int botId, string message, string userId);
    Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int botId);
    Task ClearChatHistoryAsync(int botId);
}
