using StickerAI.Infrastructure.Interfaces;
using StickerAI.Infrastructure.Models;
using StickerAI.Infrastructure.Enums;

namespace StickerAI.Infrastructure.Services;

public class StickerService
{
    private readonly IStickerGenerator _stickerGenerator;
    private readonly IStickerRepository _stickerRepository;

    public StickerService(IStickerGenerator stickerGenerator, IStickerRepository stickerRepository)
    {
        _stickerGenerator = stickerGenerator;
        _stickerRepository = stickerRepository;
    }

    public async Task<StickerModel> CreateStickerAsync(string text, CancellationToken cancellationToken = default)
    {
        var sticker = new StickerModel
        {
            Id = Guid.NewGuid(),
            Text = text,
            CreatedAt = DateTime.UtcNow,
            Status = StickerStatus.Pending
        };

        await _stickerRepository.CreateAsync(sticker, cancellationToken);

        try
        {
            var imageUrl = await _stickerGenerator.GenerateStickerAsync(text, cancellationToken);
            sticker.ImageUrl = imageUrl;
            sticker.Status = StickerStatus.Generated;
        }
        catch (Exception)
        {
            sticker.Status = StickerStatus.Failed;
        }

        await _stickerRepository.UpdateAsync(sticker, cancellationToken);
        return sticker;
    }

    public async Task<StickerModel> GetStickerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _stickerRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<StickerModel>> GetAllStickersAsync(CancellationToken cancellationToken = default)
    {
        return await _stickerRepository.GetAllAsync(cancellationToken);
    }
}