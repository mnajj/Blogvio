namespace Blogvio.WebApi.Extensions;

public static class CosmosDbInstaller
{
	//public static void InstallCosmosDb(
	//	this IServiceCollection services,
	//	IConfiguration configuration)
	//{
	//	var cosmosStoreSettings = new CosmosStoreSettings(
	//		configuration["CosmosSettings:DatabaseName"],
	//		configuration["CosmosSettings:AccountUri"],
	//		configuration["CosmosSettings:AccountKey"],
	//		new ConnectionPolicy()
	//	);

	//	services.AddCosmosStore<CosmosBlog>(cosmosStoreSettings);
	//	services.AddCosmosStore<CosmosPost>(cosmosStoreSettings);
	//	services.AddCosmosStore<CosmosComment>(cosmosStoreSettings);
	//}
}