using HighCapitalBot.Core.DTOs.Auth;

namespace HighCapitalBot.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<bool> UserExistsAsync(string email);
}
