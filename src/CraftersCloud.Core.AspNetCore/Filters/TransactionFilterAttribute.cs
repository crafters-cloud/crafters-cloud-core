using CraftersCloud.Core.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CraftersCloud.Core.AspNetCore.Filters;

public sealed class TransactionFilterAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        var unitOfWork = context.HttpContext.Resolve<IUnitOfWork>();

        if (resultContext.Exception == null &&
            context.HttpContext.Response.StatusCode is >= 200 and < 300 && context.ModelState.IsValid)
        {
            await unitOfWork.SaveChangesAsync();
        }
    }
}