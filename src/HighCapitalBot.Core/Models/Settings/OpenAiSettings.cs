namespace HighCapitalBot.Core.Models.Settings
{
    public class OpenAiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public int MaxTokens { get; set; }
        public double Temperature { get; set; }
    }
}
