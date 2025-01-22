using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.MediatR;

/// <summary>
/// Defines a behavior pipeline for MediatR that logs the handling and execution time of requests.
/// </summary>
/// <typeparam name="TRequest">The type of request being executed. Must implement <see cref="IBaseRequest"/>.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the request handler.</typeparam>
/// <remarks>
/// This behavior uses a logging mechanism to track the type of request being handled and the time taken to process it.
/// It integrates with MediatR's pipeline by wrapping around the request's execution.
/// </remarks>
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
            logger.LogDebug("Handled {RequestType} in {ElapsedMilliseconds}ms", typeName,
                stopwatch.ElapsedMilliseconds);
            return response;
        }
    }
}