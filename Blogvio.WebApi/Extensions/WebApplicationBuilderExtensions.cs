using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Blogvio.WebApi.Extensions;

public static class WebApplicationBuilderExtensions
{
	public static void AddAPIDocumentation(this WebApplicationBuilder builder)
	{
		var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(x =>
		{
			x.SwaggerDoc("v1", new OpenApiInfo { Title = "Blogvio API", Version = "v1" });
			x.IncludeXmlComments(xmlPath);
		});
	}
}
