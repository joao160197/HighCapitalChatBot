namespace HighCapitalBot.Core.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
}
