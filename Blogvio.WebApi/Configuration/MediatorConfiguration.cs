using MediatR;

namespace Blogvio.WebApi.Configuration;

public static class MediatorConfiguration
{
	public static void ConfigureMediator(this IServiceCollection services)
	{
		services.AddMediatR(typeof(Program));
	}
}
