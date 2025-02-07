﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.Core.AspNetCore;

[PublicAPI]
public static class HttpContextExtensions
{
    public static T Resolve<T>(this HttpContext httpContext) where T : notnull =>
        httpContext.RequestServices.GetRequiredService<T>();
}