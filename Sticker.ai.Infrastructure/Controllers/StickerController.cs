using Microsoft.AspNetCore.Mvc;
using StickerAI.Infrastructure.Models;
using StickerAI.Infrastructure.Services;
using Sticker.ai.Infrastructure.Exceptions;

namespace StickerAI.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StickerController : ControllerBase
{
    private readonly StickerService _stickerService;
    private readonly ILogger<StickerController> _logger;

    public StickerController(StickerService stickerService, ILogger<StickerController> logger)
    {
        _stickerService = stickerService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<StickerModel>> CreateSticker([FromBody] CreateStickerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var sticker = await _stickerService.CreateStickerAsync(request.Text, cancellationToken);
            return CreatedAtAction(nameof(GetSticker), new { id = sticker.Id }, sticker);
        }
        catch (StickerGenerationException ex)
        {
            _logger.LogError(ex, "Failed to generate sticker");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StickerModel>> GetSticker(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var sticker = await _stickerService.GetStickerAsync(id, cancellationToken);
            return Ok(sticker);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StickerModel>>> GetAllStickers(CancellationToken cancellationToken)
    {
        var stickers = await _stickerService.GetAllStickersAsync(cancellationToken);
        return Ok(stickers);
    }
}

public record CreateStickerRequest(string Text);