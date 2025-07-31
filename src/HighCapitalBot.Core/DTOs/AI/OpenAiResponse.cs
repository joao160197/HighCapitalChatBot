namespace HighCapitalBot.Core.DTOs.AI;

public class OpenAiResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public List<OpenAiChoice> Choices { get; set; } = new();
    public OpenAiUsage Usage { get; set; } = new();
}

public class OpenAiChoice
{
    public int Index { get; set; }
    public OpenAiMessage Message { get; set; } = new();
    public string? FinishReason { get; set; }
}

public class OpenAiUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}
