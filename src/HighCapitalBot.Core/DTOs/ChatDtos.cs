namespace HighCapitalBot.Core.DTOs;

public class ChatMessageDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsFromUser { get; set; }
    public DateTime Timestamp { get; set; }
    public int BotId { get; set; }
}

public class ChatRequestDto
{
    public string Message { get; set; } = string.Empty;
}

public class ChatResponseDto
{
    public ChatMessageDto UserMessage { get; set; } = null!;
    public ChatMessageDto BotResponse { get; set; } = null!;
}
