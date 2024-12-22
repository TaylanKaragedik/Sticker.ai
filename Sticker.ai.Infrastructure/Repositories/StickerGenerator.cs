using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using Sticker.ai.Infrastructure.Exceptions;
using StickerAI.Infrastructure.Configuration;
using StickerAI.Infrastructure.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace StickerAI.Infrastructure.Repositories;

public class StickerGenerator : IStickerGenerator
{
    private readonly OpenAIClient _openAiClient;
    private readonly HttpClient _httpClient;
    
    // WhatsApp sticker requirements
    private const int MaxFileSize = 100 * 1024; // 100KB
    private const int TargetSize = 512;         // 512x512 pixels

    public StickerGenerator(IOptions<AppSettings> settings)
    {
        _openAiClient = new OpenAIClient(settings.Value.OpenAiApiKey);
        _httpClient = new HttpClient();
    }

    public async Task<byte[]> GenerateStickerAsync(string text, CancellationToken cancellationToken = default)
    {
        try
        {
            var imageResponse = await _openAiClient.GetImageGenerationsAsync(
                new ImageGenerationOptions
                {
                    DeploymentName = "dall-e-3",
                    Prompt = $"A sticker design of: {text}",
                    Size = ImageSize.Size512x512,
                    Quality = "standard"
                }, 
                cancellationToken);

            // Download the image
            var imageBytes = await _httpClient.GetByteArrayAsync(imageResponse.Value.Data[0].Url, cancellationToken);
            
            // Process for WhatsApp
            return await ProcessForWhatsAppAsync(imageBytes);
        }
        catch (Exception ex)
        {
            throw new StickerGenerationException("Failed to generate sticker", ex);
        }
    }

    private async Task<byte[]> ProcessForWhatsAppAsync(byte[] originalImage)
    {
        using var image = Image.Load(originalImage);
        
        // Resize to 512x512 if needed
        if (image.Width != TargetSize || image.Height != TargetSize)
        {
            image.Mutate(x => x.Resize(TargetSize, TargetSize));
        }

        // Save as PNG with compression
        using var outputStream = new MemoryStream();
        await image.SaveAsPngAsync(outputStream, new PngEncoder
        {
            CompressionLevel = PngCompressionLevel.BestCompression
        });

        var processedImage = outputStream.ToArray();

        // Check if image meets WhatsApp size requirement
        if (processedImage.Length > MaxFileSize)
        {
            throw new StickerGenerationException("Sticker exceeds WhatsApp's 100KB limit");
        }

        return processedImage;
    }

    Task<string> IStickerGenerator.GenerateStickerAsync(string text, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
