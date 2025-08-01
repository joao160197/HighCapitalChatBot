using HighCapitalBot.API.Filters;
using HighCapitalBot.Core.DTOs;
using HighCapitalBot.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HighCapitalBot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[JwtAuthorize]
public class BotsController : ControllerBase
{
    private readonly IBotService _botService;
    private readonly ILogger<BotsController> _logger;

    public BotsController(IBotService botService, ILogger<BotsController> logger)
    {
        _botService = botService ?? throw new ArgumentNullException(nameof(botService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BotDto>>> GetAllBots()
    {
        try
        {
            var bots = await _botService.GetAllBotsAsync();
            return Ok(bots);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all bots");
            return StatusCode(500, "Internal server error while retrieving bots");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BotDto>> GetBotById(int id)
    {
        try
        {
            var bot = await _botService.GetBotByIdAsync(id);
            if (bot == null)
            {
                return NotFound($"Bot with id {id} not found.");
            }
            return Ok(bot);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bot with id {id}", id);
            return StatusCode(500, "Internal server error while retrieving the bot");
        }
    }

    [HttpPost]
    public async Task<ActionResult<BotDto>> CreateBot([FromBody] CreateBotDto createBotDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bot = await _botService.CreateBotAsync(createBotDto);
            return CreatedAtAction(nameof(GetBotById), new { id = bot.Id }, bot);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bot");
            return StatusCode(500, "Internal server error while creating the bot");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBot(int id, [FromBody] UpdateBotDto updateBotDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _botService.UpdateBotAsync(id, updateBotDto);
            if (!result)
            {
                return NotFound($"Bot with id {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bot with id {id}", id);
            return StatusCode(500, "Internal server error while updating the bot");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBot(int id)
    {
        try
        {
            var result = await _botService.DeleteBotAsync(id);
            if (!result)
            {
                return NotFound($"Bot with id {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bot with id {id}", id);
            return StatusCode(500, "Internal server error while deleting the bot");
        }
    }
}
