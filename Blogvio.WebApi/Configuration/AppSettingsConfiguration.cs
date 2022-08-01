namespace Blogvio.WebApi.Configuration;

public static class AppSettingsConfiguration
{
	public static void ConfigureAppSettingsFile(this IServiceCollection services, ConfigurationManager configuration)
	{
		var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
		configuration
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile($"appsettings.json")
			.AddJsonFile($"appsettings.{env}.json");
	}
}
