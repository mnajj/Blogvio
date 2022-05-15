using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Blogvio.WebApi.Data
{
	public static class PrebDb
	{
		public static void PrepSqlServerDatabase(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				MigrateSqlServer(serviceScope.ServiceProvider.GetService<AppDbContext>());
			}
		}

		private static void MigrateSqlServer(AppDbContext context)
		{
			Log.Information("Applying Migrations...");
			try
			{
				context.Database.Migrate();
				Log.Information("Migrations applied successfully");
			}
			catch (Exception ex)
			{
				Log.Information($"Couldn't run migrations: {ex.Message}");
			}
		}
	}
}
