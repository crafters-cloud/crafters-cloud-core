using Microsoft.AspNetCore.Routing;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

/// <summary>
/// Marker interface for all the endpoints.
/// </summary>
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}