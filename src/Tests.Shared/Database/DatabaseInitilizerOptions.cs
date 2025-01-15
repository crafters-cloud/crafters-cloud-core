namespace CraftersCloud.Core.Tests.Shared.Database;

public record DatabaseInitializerOptions
{
    public ResetDataOptions ResetDataOptions { get; init; } = new();
}