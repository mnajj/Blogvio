using Blogvio.WebApi.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blogvio.WebApi.Filters;

public class ValidationFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		if (!context.ModelState.IsValid)
		{
			var modelStateErrors = context.ModelState
				.Where(x => x.Value.Errors.Count > 0)
				.ToDictionary(d => d.Key, d => d.Value.Errors.Select(x => x.ErrorMessage))
				.ToArray();
			var errorResponse = new ErrorResponse();
			foreach (var error in modelStateErrors)
			{
				foreach (var subError in error.Value)
				{
					var errModel = new ErrorModel()
					{
						FieldName = error.Key,
						Message = subError
					};
					errorResponse.Errors.Add(errModel);
				}
			}

			context.Result = new BadRequestObjectResult(errorResponse);
			return;
		}

		await next();
	}
}