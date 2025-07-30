using System.ComponentModel.DataAnnotations;

namespace HighCapitalBot.Core.DTOs;

public class BotDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string InitialContext { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateBotDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string InitialContext { get; set; } = string.Empty;
}

public class UpdateBotDto
{
    [StringLength(100)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? InitialContext { get; set; }
}
