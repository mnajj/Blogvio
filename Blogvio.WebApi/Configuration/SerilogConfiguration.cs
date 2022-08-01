using Serilog;
using Serilog.Exceptions;

namespace Blogvio.WebApi.Configuration;

public static class SerilogConfiguration
{
	public static void ConfigureSerilog(this IServiceCollection services, ConfigureHostBuilder host)
	{
		Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.Enrich.WithExceptionDetails()
			.WriteTo.Debug()
			.WriteTo.Console()
			.CreateLogger();
		host.UseSerilog();
	}
}
