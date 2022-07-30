using Blogvio.WebApi.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blogvio.WebApi.Infrastructure.Filters;

public class ValidationFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		if (!context.ModelState.IsValid)
		{
			throw new ModelValidationException();
		}
		await next();
	}
}