namespace CraftersCloud.Core.TestUtilities.Database;

public record DatabaseInitializerOptions
{
    public ResetDataOptions ResetDataOptions { get; init; } = new();
}