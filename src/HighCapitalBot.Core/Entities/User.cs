using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HighCapitalBot.Core.Entities;

public class User : IdentityUser<int>
{
    // Propriedades padrão do IdentityUser já incluem:
    // - Id (int)
    // - UserName
    // - Email
    // - PasswordHash
    // - etc.
    
    public override string? Email { get; set; }
    public override string? UserName { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation property
    public ICollection<Bot> Bots { get; set; } = new List<Bot>();
}
