using Blogvio.WebApi.Repositories;
using Blogvio.WebApi.Repositories.IRepository;
using Blogvio.WebApi.Repositories.Repository.SQLServer;
using Blogvio.WebApi.Services;

namespace Blogvio.WebApi.Extensions;

public static class Services
{
	public static void RegisterServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<IBlogRepository, BlogRepository>();
		builder.Services.AddScoped<IPostRepository, PostRepository>();
		builder.Services.AddSingleton<IUriService>(provider =>
		{
			var request = provider
				.GetRequiredService<IHttpContextAccessor>()
				.HttpContext
				.Request;
			var absoluteUri = string.Concat(
				request.Scheme, "://", request.Host.ToUriComponent(), "/");
			return new UriService(absoluteUri);
		});

		// builder.Services.Scan(s =>
		// 	s.FromCallingAssembly()
		// 		.AddClasses()
		// 		.AsMatchingInterface()
		// 		.WithScopedLifetime());
	}
}