using Microsoft.EntityFrameworkCore;
using StickerAI.Infrastructure.Data;
using StickerAI.Infrastructure.Interfaces;
using StickerAI.Infrastructure.Models;

namespace StickerAI.Infrastructure.Repositories;

public class StickerRepository : IStickerRepository
{
    private readonly ApplicationDbContext _context;

    public StickerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StickerModel> CreateAsync(StickerModel sticker, CancellationToken cancellationToken = default)
    {
        await _context.Stickers.AddAsync(sticker, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sticker;
    }

    public async Task<StickerModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Stickers
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"Sticker with ID {id} not found");
    }

    public async Task<IEnumerable<StickerModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stickers
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(StickerModel sticker, CancellationToken cancellationToken = default)
    {
        _context.Stickers.Update(sticker);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
