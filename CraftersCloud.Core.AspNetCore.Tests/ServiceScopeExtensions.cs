﻿using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.Core.AspNetCore.Tests;

public static class ServiceScopeExtensions
{
    public static T Resolve<T>(this IServiceScope scope) where T : notnull =>
        scope.ServiceProvider.GetRequiredService<T>();
}