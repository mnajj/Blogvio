using Microsoft.EntityFrameworkCore;

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
			Console.WriteLine("--> Applying Migrations...");
			try
			{
				context.Database.Migrate();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Couldn't run migrations: {ex.Message}");
			}
		}
	}
}
