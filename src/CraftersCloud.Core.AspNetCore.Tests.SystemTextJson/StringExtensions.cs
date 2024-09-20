using System.Text.Json;
using CraftersCloud.Core.AspNetCore.Tests.SystemTextJson.Http;

namespace CraftersCloud.Core.AspNetCore.Tests.SystemTextJson;

internal static class StringExtensions
{
    internal static T? Deserialize<T>(this string content) =>
        JsonSerializer.Deserialize<T>(content, HttpSerializationOptions.Options);
}
