using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Configuration;

public static class ApiBehaviorConfiguration
{
	public static void ConfigureApiBehavior(this IServiceCollection services)
	{
		services.Configure<ApiBehaviorOptions>(options =>
		{
			options.SuppressModelStateInvalidFilter = true;
		});
	}
}
