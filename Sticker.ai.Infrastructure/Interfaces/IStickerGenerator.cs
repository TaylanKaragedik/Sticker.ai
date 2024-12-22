namespace StickerAI.Infrastructure.Interfaces;

public interface IStickerGenerator
{
    Task<string> GenerateStickerAsync(string text, CancellationToken cancellationToken = default);
}