using System.Diagnostics;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CraftersCloud.Core.AspNetCore;

[PublicAPI]
public static class HttpContextExtensions
{
    public static T Resolve<T>(this HttpContext httpContext) where T : notnull =>
        httpContext.RequestServices.GetRequiredService<T>();
}