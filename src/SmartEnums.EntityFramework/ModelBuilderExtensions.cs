using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;

namespace CraftersCloud.Core.SmartEnums.EntityFramework;

[PublicAPI]
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configure SmartEnums for EntityFramework
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void CoreConfigureSmartEnums(this ModelBuilder modelBuilder)
        => modelBuilder.ConfigureSmartEnum();
}