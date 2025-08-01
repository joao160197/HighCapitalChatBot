using System.ComponentModel.DataAnnotations;

namespace HighCapitalBot.Core.DTOs;

public class SendMessageDto
{
    [Required]
    public int BotId { get; set; }
    
    [Required]
    [StringLength(1000, ErrorMessage = "A mensagem não pode ter mais de 1000 caracteres.")]
    public string Message { get; set; } = string.Empty;
}
