namespace HighCapitalBot.Core.Configuration;

public class OpenAISettings
{
    public const string SectionName = "OpenAISettings";
    
    public string ApiKey { get; set; } = string.Empty;
    public string ModelName { get; set; } = "gpt-3.5-turbo";
    public int MaxTokens { get; set; } = 1000;
    public double Temperature { get; set; } = 0.7;
}
