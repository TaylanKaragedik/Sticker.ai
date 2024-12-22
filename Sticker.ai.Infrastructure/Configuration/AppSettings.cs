namespace StickerAI.Infrastructure.Configuration;

public class AppSettings
{
    public required string DatabaseConnection { get; set; }
    public required AiServiceSettings AiService { get; set; }
    public required string OpenAiApiKey { get; set; }
}

public class AiServiceSettings
{
    public required string ApiKey { get; set; }
    public required string BaseUrl { get; set; }
    public int TimeoutSeconds { get; set; }
}