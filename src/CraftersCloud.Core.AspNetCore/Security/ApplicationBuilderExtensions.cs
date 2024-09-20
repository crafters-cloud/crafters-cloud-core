using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace CraftersCloud.Core.AspNetCore.Security;

[PublicAPI]
public static class ApplicationBuilderExtensions
{
    public static void UsyCoreHttps(this IApplicationBuilder builder, IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            builder.UseHsts();
        }

        builder.UseHttpsRedirection();
    }
}