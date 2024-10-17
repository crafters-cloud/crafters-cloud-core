using System.Text.Json;

namespace CraftersCloud.Core.AspNetCore.TestUtilities.Http;

public static class HttpSerializationOptions
{
    public static JsonSerializerOptions Options { get; set; } = new()
    {
        PropertyNameCaseInsensitive = true
    };
}