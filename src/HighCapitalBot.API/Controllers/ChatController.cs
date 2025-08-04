using HighCapitalBot.API.Filters;
using HighCapitalBot.Core.DTOs;
using HighCapitalBot.Core.DTOs.Message;
using HighCapitalBot.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HighCapitalBot.API.Controllers;

[ApiController]
[Route("api/chat")]
[JwtAuthorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IChatService chatService, ILogger<ChatController> logger)
    {
        _chatService = chatService ?? throw new ArgumentNullException(nameof(chatService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("{botId}/message")]
    public async Task<IActionResult> SendMessage(int botId, [FromBody] SendMessageRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token.");
            }

            await _chatService.SendMessageAsync(botId, request.Content, userId);
            var history = await _chatService.GetChatHistoryAsync(botId);
            return Ok(history);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to bot {botId}", botId);
            return StatusCode(500, "Internal server error while processing the message");
        }
    }

    [HttpGet("{botId}/history")]
    public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetChatHistory(int botId)
    {
        try
        {
            var messages = await _chatService.GetChatHistoryAsync(botId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting chat history for bot {botId}", botId);
            return StatusCode(500, "Internal server error while retrieving chat history");
        }
    }

    [HttpDelete("{botId}/history")]
    public async Task<IActionResult> ClearChatHistory(int botId)
    {
        try
        {
            await _chatService.ClearChatHistoryAsync(botId);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing chat history for bot {botId}", botId);
            return StatusCode(500, "Internal server error while clearing chat history");
        }
    }
}
