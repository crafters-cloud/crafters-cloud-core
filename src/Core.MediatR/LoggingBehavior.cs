using System.Diagnostics;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.MediatR;

[PublicAPI]
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();
       
        using (logger.BeginScope(new Dictionary<string, object> { { "MediatRRequestType", typeName } }))
        {
            logger.LogDebug("Handling {RequestType}", typeName);
            var stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();
            logger.LogDebug("Handled {RequestType} in {ElapsedMilliseconds}ms", typeName, stopwatch.ElapsedMilliseconds);
            return response;
        }
    }
}