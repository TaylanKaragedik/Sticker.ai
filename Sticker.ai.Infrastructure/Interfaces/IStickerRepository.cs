using StickerAI.Infrastructure.Models;

namespace StickerAI.Infrastructure.Interfaces;

public interface IStickerRepository
{
    Task<StickerModel> CreateAsync(StickerModel sticker, CancellationToken cancellationToken = default);
    Task<StickerModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<StickerModel>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(StickerModel sticker, CancellationToken cancellationToken = default);
}