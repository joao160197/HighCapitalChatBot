namespace HighCapitalBot.Core.DTOs.AI;

public class OpenAiRequest
{
    public string Model { get; set; } = string.Empty;
    public List<OpenAiMessage> Messages { get; set; } = new();
    public double Temperature { get; set; }
    public int MaxTokens { get; set; }
}
