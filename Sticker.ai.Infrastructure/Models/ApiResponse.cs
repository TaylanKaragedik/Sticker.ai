namespace StickerAI.Infrastructure.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public required List<string> Errors { get; set; }
    public required T? Data { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string message = "Operation completed successfully")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = new List<string>()
        };
    }

    public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>(),
            Data = default
        };
    }
} 