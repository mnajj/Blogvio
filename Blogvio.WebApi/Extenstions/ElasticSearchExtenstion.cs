using Blogvio.WebApi.Models;
using Nest;

namespace Blogvio.WebApi.Extenstions
{
	public static class ElasticSearchExtenstion
	{
		public static void AddElasticSearch(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			var url = configuration["ELKConfiguration:Uri"];
			var defaultIndex = configuration["ELKConfiguration:Index"];

			var settings = new ConnectionSettings(new Uri(url))
				.PrettyJson()
				.DefaultIndex(defaultIndex);

			AddDefaultMapping(settings);

			var client = new ElasticClient(settings);
			services.AddSingleton<IElasticClient>(client);
			CreateIndex(client, defaultIndex);
		}

		private static void AddDefaultMapping(ConnectionSettings settings)
		{
			settings.DefaultMappingFor<Post>(p =>
				p.Ignore(x => x.Id)
				.Ignore(x => x.IsDeleted)
				.Ignore(x => x.PublishedAt)
				.Ignore(x => x.UpdatedAt)
				.Ignore(x => x.BlogId));
		}

		private static void CreateIndex(IElasticClient client, string indexName)
		{
			client.Indices.Create(indexName, i => i.Map<Post>(x => x.AutoMap()));
		}
	}
}
