﻿using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CraftersCloud.Core.AspNetCore.Exceptions;

[PublicAPI]
public static class ApplicationBuilderExtensions
{
    public static void UseCoreExceptionHandler(this IApplicationBuilder builder,
        Func<HttpContext, Task<bool>>? onBeforeException = null) =>
        builder.UseExceptionHandler(new ExceptionHandlerOptions
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async context =>
            {
                var handledElsewhere = false;
                if (onBeforeException != null)
                {
                    handledElsewhere = await onBeforeException.Invoke(context);
                }

                if (handledElsewhere)
                {
                    return;
                }

                await ExceptionHandler.HandleExceptionFrom(context);
            }
        });
}