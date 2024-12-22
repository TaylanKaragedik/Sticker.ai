namespace Sticker.ai.Infrastructure.Exceptions;

public class StickerGenerationException : Exception
{
    public StickerGenerationException(string message) : base(message)
    {
    }

    public StickerGenerationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
} 