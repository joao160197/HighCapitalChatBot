using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoMapper;
using HighCapitalBot.Core.Configuration;
using HighCapitalBot.Core.Data;
using HighCapitalBot.Core.DTOs;
using HighCapitalBot.Core.DTOs.AI;
using HighCapitalBot.Core.Entities;
using HighCapitalBot.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HighCapitalBot.Core.Services;

public class ChatService : IChatService
{
    private readonly IRepository<ChatMessage> _chatMessageRepository;
    private readonly IRepository<Bot> _botRepository;
    private readonly IAiService _aiService;
    private readonly ILogger<ChatService> _logger;
    private readonly IMapper _mapper;

    public ChatService(
        IRepository<ChatMessage> chatMessageRepository,
        IRepository<Bot> botRepository,
        IAiService aiService,
        ILogger<ChatService> logger,
        IMapper mapper)
    {
        _chatMessageRepository = chatMessageRepository ?? throw new ArgumentNullException(nameof(chatMessageRepository));
        _botRepository = botRepository ?? throw new ArgumentNullException(nameof(botRepository));
        _aiService = aiService ?? throw new ArgumentNullException(nameof(aiService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ChatResponseDto> SendMessageAsync(int botId, string message)
    {
        try
        {
            // Get the bot with its context
            var bot = await _botRepository.GetByIdAsync(botId);
            if (bot == null)
            {
                throw new ArgumentException($"Bot with id {botId} not found.", nameof(botId));
            }

            // Save user message
            var userMessage = new ChatMessage
            {
                Content = message,
                IsFromUser = true,
                Timestamp = DateTime.UtcNow,
                BotId = botId
            };

            await _chatMessageRepository.AddAsync(userMessage);

            // Get chat history for context
            var chatHistory = await GetChatHistoryForContext(botId);
            
            // Prepare chat history for AI
            var messages = new List<OpenAiMessage>
            {
                new() { Role = "system", Content = bot.InitialContext }
            };

            // Add chat history
            foreach (var chatMessage in chatHistory)
            {
                messages.Add(new OpenAiMessage
                {
                    Role = chatMessage.IsFromUser ? "user" : "assistant",
                    Content = chatMessage.Content
                });
            }

            // Add the new message
            messages.Add(new OpenAiMessage { Role = "user", Content = message });

            // Call AI service with history
            var botResponseContent = await _aiService.GetResponseWithHistoryAsync(messages);

            // Save bot response
            var botMessage = new ChatMessage
            {
                Content = botResponseContent,
                IsFromUser = false,
                Timestamp = DateTime.UtcNow,
                BotId = botId
            };

            await _chatMessageRepository.AddAsync(botMessage);
            await _chatMessageRepository.SaveChangesAsync();

            // Return both messages
            return new ChatResponseDto
            {
                UserMessage = _mapper.Map<ChatMessageDto>(userMessage),
                BotResponse = _mapper.Map<ChatMessageDto>(botMessage)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending message to bot {botId}", botId);
            throw;
        }
    }

    public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int botId)
    {
        try
        {
            var messages = await _chatMessageRepository.FindAsync(m => m.BotId == botId);
            return _mapper.Map<IEnumerable<ChatMessageDto>>(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting chat history for bot {botId}", botId);
            throw;
        }
    }

    public async Task ClearChatHistoryAsync(int botId)
    {
        try
        {
            var messages = await _chatMessageRepository.FindAsync(m => m.BotId == botId);
            _chatMessageRepository.RemoveRange(messages);
            await _chatMessageRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error clearing chat history for bot {botId}", botId);
            throw;
        }
    }

    private async Task<IEnumerable<ChatMessage>> GetChatHistoryForContext(int botId, int maxMessages = 10)
    {
        return await _chatMessageRepository
            .FindAsync(m => m.BotId == botId)
            .ContinueWith(task => task.Result
                .OrderByDescending(m => m.Timestamp)
                .Take(maxMessages)
                .OrderBy(m => m.Timestamp));
    }

    // Método removido pois a lógica foi movida para o OpenAiService
}
