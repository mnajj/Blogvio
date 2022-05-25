using Cosmonaut.Attributes;
using Newtonsoft.Json;

namespace Blogvio.WebApi.Models.CosmosModels
{
	[CosmosCollection("posts")]
	public class CosmosPost
	{
		[CosmosPartitionKey]
		[JsonProperty("id")]
		public int Id { get; set; }

		public string Content { get; set; }

		public DateTime PublishedAt { get; set; } = DateTime.Now;

		public DateTime? UpdatedAt { get; set; }

		public bool IsDeleted { get; set; }

		public int BlogId { get; set; }

		public ICollection<int> Comments { get; set; }
	}
}
