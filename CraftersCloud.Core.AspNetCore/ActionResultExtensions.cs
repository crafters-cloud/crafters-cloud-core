using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CraftersCloud.Core.AspNetCore;

public static class ActionResultExtensions
{
    public static ActionResult<TDestination> ToActionResult<TDestination>(this TDestination? model)
        where TDestination : class =>
        model == null
            ? new NotFoundResult()
            : new OkObjectResult(model);

    public static ActionResult<TDestination> MapToActionResult<TDestination>(this IMapper mapper, object? value) =>
        value == null
            ? new NotFoundResult()
            : new OkObjectResult(mapper.Map<TDestination>(value));
}