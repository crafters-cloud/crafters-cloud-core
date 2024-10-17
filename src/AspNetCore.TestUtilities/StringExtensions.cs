using System.Text.Json;
using CraftersCloud.Core.AspNetCore.TestUtilities.Http;

namespace CraftersCloud.Core.AspNetCore.TestUtilities;

internal static class StringExtensions
{
    internal static T? Deserialize<T>(this string content) =>
        JsonSerializer.Deserialize<T>(content, HttpSerializationOptions.Options);
}