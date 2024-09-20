namespace CraftersCloud.Core.TestUtils.Database;

public record ResetDataOptions
{
    public IEnumerable<string> TablesToIgnore { get; init; } = [];
    public string CustomSqlQuery { get; init; } = string.Empty;
    public bool ReseedIdentityColumns { get; init; } = true;
}