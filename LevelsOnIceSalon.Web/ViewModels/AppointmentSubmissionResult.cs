namespace LevelsOnIceSalon.Web.ViewModels;

public sealed class AppointmentSubmissionResult
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public static AppointmentSubmissionResult Successful(string message) => new()
    {
        Success = true,
        Message = message
    };

    public static AppointmentSubmissionResult Failure(string message) => new()
    {
        Success = false,
        Message = message
    };
}
