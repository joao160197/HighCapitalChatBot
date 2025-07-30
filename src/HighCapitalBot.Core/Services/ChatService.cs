using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HighCapitalBot.Core.Configuration;
using HighCapitalBot.Core.Data;
using HighCapitalBot.Core.DTOs;
using HighCapitalBot.Core.Entities;
using HighCapitalBot.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HighCapitalBot.Core.Services;

public class ChatService : IChatService
{
    private readonly IRepository<ChatMessage> _chatMessageRepository;
    private readonly IRepository<Bot> _botRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly OpenAISettings _openAISettings;
    private readonly ILogger<ChatService> _logger;
    private readonly IMapper _mapper;

    public ChatService(
        IRepository<ChatMessage> chatMessageRepository,
        IRepository<Bot> botRepository,
        IHttpClientFactory httpClientFactory,
        IOptions<OpenAISettings> openAISettings,
        ILogger<ChatService> logger,
        IMapper mapper)
    {
        _chatMessageRepository = chatMessageRepository ?? throw new ArgumentNullException(nameof(chatMessageRepository));
        _botRepository = botRepository ?? throw new ArgumentNullException(nameof(botRepository));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _openAISettings = openAISettings?.Value ?? throw new ArgumentNullException(nameof(openAISettings));
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
            
            // Call OpenAI API
            var botResponseContent = await CallOpenAIChatAPIAsync(bot, chatHistory, message);

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
            _logger.LogError(ex, $"Error sending message to bot {BotId}", botId);
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
            _logger.LogError(ex, $"Error getting chat history for bot {BotId}", botId);
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
            _logger.LogError(ex, $"Error clearing chat history for bot {BotId}", botId);
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

    private async Task<string> CallOpenAIChatAPIAsync(Bot bot, IEnumerable<ChatMessage> chatHistory, string newMessage)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAISettings.ApiKey);

        // Prepare messages with bot context and chat history
        var messages = new List<object>
        {
            new { role = "system", content = bot.InitialContext }
        };

        // Add chat history
        foreach (var message in chatHistory)
        {
            messages.Add(new
            {
                role = message.IsFromUser ? "user" : "assistant",
                content = message.Content
            });
        }

        // Add the new message
        messages.Add(new { role = "user", content = newMessage });

        // Prepare request body
        var requestBody = new
        {
            model = _openAISettings.ModelName,
            messages = messages,
            max_tokens = _openAISettings.MaxTokens,
            temperature = _openAISettings.Temperature
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        // Send request to OpenAI API
        var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        response.EnsureSuccessStatusCode();

        // Parse response
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);
        
        return responseObject
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;
    }
}
