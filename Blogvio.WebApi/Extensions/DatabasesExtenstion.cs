using Blogvio.WebApi.Data;
using Blogvio.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Blogvio.WebApi.Extensions;

public static class DatabasesExtenstion
{
	public static void RegisterAllDbs(this WebApplicationBuilder builder)
	{
		builder.AddSqlServer();
		builder.AddElasticSearch();
		builder.AddRedisDistributedCahce();
	}

	public static void AddElasticSearch(this WebApplicationBuilder builder)
	{
		var url = builder.Configuration["ELKConfiguration:Uri"];
		var defaultIndex = builder.Configuration["ELKConfiguration:Index"];

		var settings = new ConnectionSettings(new Uri(url))
			.PrettyJson()
			.DefaultIndex(defaultIndex);

		AddDefaultMapping(settings);

		var client = new ElasticClient(settings);
		builder.Services.AddSingleton<IElasticClient>(client);
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

	public static void AddSqlServer(this WebApplicationBuilder builder)
	{
		builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
	}

	public static void AddRedisDistributedCahce(this WebApplicationBuilder builder)
	{
		var connectionString = builder.Configuration["Redis:ConnectionString"];
		builder.Services.AddStackExchangeRedisCache(opts =>
		{
			opts.Configuration = connectionString;
			opts.InstanceName = "Blogvio";
		});
	}
}