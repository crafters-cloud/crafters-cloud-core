using System.Reflection;
using JetBrains.Annotations;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[PublicAPI]
public class EntitiesDbContextOptions
{
    public Assembly ConfigurationAssembly { get; set; } = default!;

    public Assembly EntitiesAssembly { get; set; } = default!;
}