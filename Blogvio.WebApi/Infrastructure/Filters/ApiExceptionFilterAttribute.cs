using Blogvio.WebApi.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blogvio.WebApi.Infrastructure.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
	private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

	public ApiExceptionFilterAttribute()
	{
		_exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
		{
			{ typeof(EntityNotFoundException), HandleNotFoundException },
			{ typeof(ModelValidationException), HandleInvalidModelStateException },
			{ typeof(DbCommitFailException), HandleDbCommitFailException },
		};
	}

	public override void OnException(ExceptionContext context)
	{
		HandleException(context);
		base.OnException(context);
	}

	private void HandleException(ExceptionContext context)
	{
		var type = context.Exception.GetType();
		if (_exceptionHandlers.ContainsKey(type))
		{
			_exceptionHandlers[type].Invoke(context);
			return;
		}
		if (!context.ModelState.IsValid)
		{
			HandleInvalidModelStateException(context);
			return;
		}
		HandleUnknownException(context);
	}

	private void HandleUnknownException(ExceptionContext context)
	{
		var details = new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "An error occurred while processing your request.",
			Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
		};

		context.Result = new ObjectResult(details)
		{
			StatusCode = StatusCodes.Status500InternalServerError
		};
		context.ExceptionHandled = true;
	}

	private void HandleNotFoundException(ExceptionContext context)
	{
		var exception = context.Exception as EntityNotFoundException;
		var details = new ProblemDetails()
		{
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "The specified resource was not found.",
			Detail = exception?.Message
		};
		context.Result = new NotFoundObjectResult(details);
		context.ExceptionHandled = true;
	}

	private void HandleInvalidModelStateException(ExceptionContext context)
	{
		var details = new ValidationProblemDetails(context.ModelState)
		{
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
		};
		context.Result = new BadRequestObjectResult(details);
		context.ExceptionHandled = true;
	}

	private void HandleDbCommitFailException(ExceptionContext context)
	{
		var details = new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "An error occurred while saving your resource.",
			Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
		};
		context.Result = new ObjectResult(details)
		{
			StatusCode = StatusCodes.Status500InternalServerError
		};
		context.ExceptionHandled = true;
	}
}
