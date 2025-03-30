using System.Diagnostics;

namespace EmailManagement.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string? ErrorMessage { get; set; }

    public string? ExceptionDetails { get; set; }

    public static ErrorViewModel FromException(Exception ex, string? requestId = null)
    {
        return new ErrorViewModel
        {
            RequestId = requestId ?? Activity.Current?.Id ?? "Unknown",
            ErrorMessage = ex.Message,
            ExceptionDetails = ex.ToString()
        };
    }
}