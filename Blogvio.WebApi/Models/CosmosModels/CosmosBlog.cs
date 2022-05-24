using Cosmonaut.Attributes;
using Newtonsoft.Json;

namespace Blogvio.WebApi.Models.CosmosModels
{
	[CosmosCollection("blogs")]
	public class CosmosBlog
	{
		[CosmosPartitionKey]
		[JsonProperty("id")]
		public string Id { get; set; }

		public string Url { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public bool IsDeleted { get; set; }
	}
}
