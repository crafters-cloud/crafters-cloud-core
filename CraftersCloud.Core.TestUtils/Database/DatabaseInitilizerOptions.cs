namespace CraftersCloud.Core.TestUtils.Database;

public record DatabaseInitializerOptions
{
    public ResetDataOptions ResetDataOptions { get; init; } = new();
}