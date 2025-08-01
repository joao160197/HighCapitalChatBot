using Newtonsoft.Json;
using System.Collections.Generic;

namespace HighCapitalBot.Core.DTOs.AI
{
    public class HuggingFaceResponse
    {
        [JsonProperty("generated_text")]
        public string? GeneratedText { get; set; }
    }
}
