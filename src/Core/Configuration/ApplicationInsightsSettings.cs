namespace CraftersCloud.Core.Configuration;

[PublicAPI]
public class ApplicationInsightsSettings
{
    public const string SectionName = "ApplicationInsights";

    public string ConnectionString { get; set; } = string.Empty;
}