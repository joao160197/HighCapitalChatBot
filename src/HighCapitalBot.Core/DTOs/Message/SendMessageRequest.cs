using System.ComponentModel.DataAnnotations;

namespace HighCapitalBot.Core.DTOs.Message;

public class SendMessageRequest
{
    [Required]
    public string Content { get; set; } = string.Empty;
}
