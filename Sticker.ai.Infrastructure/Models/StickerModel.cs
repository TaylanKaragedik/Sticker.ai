using StickerAI.Infrastructure.Enums;

namespace StickerAI.Infrastructure.Models;

public class StickerModel
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public StickerStatus Status { get; set; }
}