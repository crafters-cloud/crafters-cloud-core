using JetBrains.Annotations;
using Serilog.Events;

namespace CraftersCloud.Core.Settings;

[PublicAPI]
public class ApplicationInsightsSettings
{
    public const string ApplicationInsightsSectionName = "ApplicationInsights";
    
    public string ConnectionString { get; set; } = string.Empty;
}