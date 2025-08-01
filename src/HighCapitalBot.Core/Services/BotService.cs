using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HighCapitalBot.Core.Data;
using HighCapitalBot.Core.DTOs;
using HighCapitalBot.Core.Entities;
using HighCapitalBot.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HighCapitalBot.Core.Services;

public class BotService : IBotService
{
    private readonly IRepository<Bot> _botRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BotService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BotService(IRepository<Bot> botRepository, IMapper mapper, ILogger<BotService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _botRepository = botRepository ?? throw new ArgumentNullException(nameof(botRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<BotDto> CreateBotAsync(CreateBotDto createBotDto)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User ID not found in token.");
                throw new UnauthorizedAccessException("User ID not found in token.");
            }

            var bot = _mapper.Map<Bot>(createBotDto);
            bot.AppUserId = userId;
            await _botRepository.AddAsync(bot);
            await _botRepository.SaveChangesAsync();
            
            return _mapper.Map<BotDto>(bot);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bot");
            throw;
        }
    }

    public async Task<BotDto?> GetBotByIdAsync(int id)
    {
        try
        {
            var bot = await _botRepository.GetByIdAsync(id);
            return bot == null ? null : _mapper.Map<BotDto>(bot);
        }
        catch (Exception ex)
        {
            // Corrigido para usar o log estruturado corretamente
            _logger.LogError(ex, "Error getting bot with id {id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<BotDto>> GetAllBotsAsync()
    {
        try
        {
            var bots = await _botRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BotDto>>(bots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all bots");
            throw;
        }
    }

    public async Task<bool> UpdateBotAsync(int id, UpdateBotDto updateBotDto)
    {
        try
        {
            var bot = await _botRepository.GetByIdAsync(id);
            if (bot == null) return false;

            if (!string.IsNullOrWhiteSpace(updateBotDto.Name))
                bot.Name = updateBotDto.Name;
                
            if (!string.IsNullOrWhiteSpace(updateBotDto.Description))
                bot.Description = updateBotDto.Description;
                
            if (!string.IsNullOrWhiteSpace(updateBotDto.InitialContext))
                bot.InitialContext = updateBotDto.InitialContext;
                
            bot.UpdatedAt = DateTime.UtcNow;
            
            _botRepository.Update(bot);
            return await _botRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Corrigido para usar o log estruturado corretamente
            _logger.LogError(ex, "Error updating bot with id {id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteBotAsync(int id)
    {
        try
        {
            var bot = await _botRepository.GetByIdAsync(id);
            if (bot == null) return false;
            
            _botRepository.Remove(bot);
            return await _botRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Corrigido para usar o log estruturado corretamente
            _logger.LogError(ex, "Error deleting bot with id {id}", id);
            throw;
        }
    }
}
