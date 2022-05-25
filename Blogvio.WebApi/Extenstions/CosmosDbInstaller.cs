using Blogvio.WebApi.Models.CosmosModels;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Microsoft.Azure.Documents.Client;

namespace Blogvio.WebApi.Extenstions
{
	public static class CosmosDbInstaller
	{
		public static void InstallCosmosDb(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			var cosmosStoreSettings = new CosmosStoreSettings(
				configuration["CosmosSettings:DatabaseName"],
				configuration["CosmosSettings:AccountUri"],
				configuration["CosmosSettings:AccountKey"],
				new ConnectionPolicy()
				);

			services.AddCosmosStore<CosmosBlog>(cosmosStoreSettings);
			services.AddCosmosStore<CosmosPost>(cosmosStoreSettings);
			services.AddCosmosStore<CosmosComment>(cosmosStoreSettings);
		}
	}
}
