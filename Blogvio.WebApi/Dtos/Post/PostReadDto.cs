using System.ComponentModel.DataAnnotations;

namespace Blogvio.WebApi.Dtos.Post
{
	public class PostReadDto
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(200)]
		public string Content { get; set; }

		[Required]
		public DateTime PublishedAt { get; set; }

		[Required]
		public DateTime UpdatedAt { get; set; }
	}
}
