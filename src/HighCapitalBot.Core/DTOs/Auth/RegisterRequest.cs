using System.ComponentModel.DataAnnotations;

namespace HighCapitalBot.Core.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
