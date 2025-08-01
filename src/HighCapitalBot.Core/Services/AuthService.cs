using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HighCapitalBot.Core.Configuration;
using HighCapitalBot.Core.DTOs.Auth;
using HighCapitalBot.Core.Entities;
using HighCapitalBot.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HighCapitalBot.Core.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly JwtSettings _jwtSettings;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        IRepository<User> userRepository,
        IOptions<JwtSettings> jwtSettings,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await UserExistsAsync(request.Email))
        {
            throw new InvalidOperationException("Email already registered");
        }

        var user = new User
        {
            UserName = request.Username, // Corrigido de Username para UserName
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        // Hash da senha
        user.PasswordHash = HashPassword(user, request.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return GenerateJwtToken(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Corrigido para usar FindAsync, que existe no repositório
        var user = (await _userRepository.FindAsync(u => u.Email == request.Email)).FirstOrDefault();

        if (user == null || !VerifyPassword(user, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        user.LastLoginAt = DateTime.UtcNow;
        _userRepository.Update(user); // Corrigido de UpdateAsync para Update
        await _userRepository.SaveChangesAsync();

        return GenerateJwtToken(user);
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        // Corrigido para usar FindAsync, que existe no repositório
        var users = await _userRepository.FindAsync(u => u.Email == email);
        return users.Any();
    }

    private string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    private bool VerifyPassword(User user, string password)
    {
        if (string.IsNullOrEmpty(user.PasswordHash))
            return false;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success || 
               result == PasswordVerificationResult.SuccessRehashNeeded;
    }

    private AuthResponse GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new AuthResponse
        {
            Token = tokenString,
            ExpiresAt = token.ValidTo,
            Email = user.Email ?? string.Empty,
            Username = user.UserName ?? string.Empty
        };
    }
}
