using HighCapitalBot.Core.DTOs;
using HighCapitalBot.Core.Entities;

namespace HighCapitalBot.Core.Interfaces;

public interface IBotService
{
    Task<BotDto> CreateBotAsync(CreateBotDto createBotDto);
    Task<BotDto?> GetBotByIdAsync(int id);
    Task<IEnumerable<BotDto>> GetAllBotsAsync();
    Task<bool> UpdateBotAsync(int id, UpdateBotDto updateBotDto);
    Task<bool> DeleteBotAsync(int id);
}
