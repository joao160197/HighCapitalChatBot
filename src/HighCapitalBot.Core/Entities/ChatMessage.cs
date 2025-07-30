using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HighCapitalBot.Core.Entities;

public class ChatMessage
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public bool IsFromUser { get; set; }
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    public int BotId { get; set; }
    
    // Navigation property
    [ForeignKey("BotId")]
    public Bot Bot { get; set; } = null!;
}
