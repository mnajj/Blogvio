using System.ComponentModel.DataAnnotations;

namespace Blogvio.WebApi.Dtos.Post
{
	public class PostReadDto
	{
		[Required]
		[MaxLength(200)]
		public string Content { get; set; }

		[Required]
		public DateTime PublishedAt { get; set; }

		[Required]
		public DateTime UpdatedAt { get; set; }
	}
}
