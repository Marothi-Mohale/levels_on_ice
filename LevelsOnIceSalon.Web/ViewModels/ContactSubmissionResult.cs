namespace LevelsOnIceSalon.Web.ViewModels;

public sealed class ContactSubmissionResult
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public static ContactSubmissionResult Successful(string message) => new()
    {
        Success = true,
        Message = message
    };

    public static ContactSubmissionResult Failure(string message) => new()
    {
        Success = false,
        Message = message
    };
}
