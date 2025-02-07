﻿using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CraftersCloud.Core.AspNetCore.Security;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static void AddCoreHttps(this IServiceCollection services, IHostEnvironment environment)
    {
        var isDevelopmentEnvironment = environment.IsDevelopment();

        if (!isDevelopmentEnvironment)
        {
            const int oneYear = 365;
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(oneYear);
            });
        }

        var redirection = isDevelopmentEnvironment
            ? HttpStatusCode.TemporaryRedirect
            : HttpStatusCode.PermanentRedirect;

        services.AddHttpsRedirection(options =>
        {
            options.RedirectStatusCode = (int) redirection;
        });
    }
}