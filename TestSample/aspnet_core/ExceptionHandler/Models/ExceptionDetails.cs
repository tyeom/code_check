namespace HandlingExceptions.Models;

using System.Text.Json;

public class ExceptionDetails
{
    public int StatusCode { get; set; }

    public string? Message { get; set; }

    public string? DetailMessage { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
