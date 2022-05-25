using Cosmonaut.Attributes;
using Newtonsoft.Json;

namespace Blogvio.WebApi.Models.CosmosModels
{
	[CosmosCollection("comments")]
	public class CosmosComment
	{
		[CosmosPartitionKey]
		[JsonProperty("id")]
		public string Id { get; set; }

		public string Content { get; set; }

		public DateTime PublishedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public bool IsDeleted { get; set; }

		public int PostId { get; set; }
	}
}
