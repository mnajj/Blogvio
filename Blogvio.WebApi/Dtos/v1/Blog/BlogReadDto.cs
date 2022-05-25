using System.ComponentModel.DataAnnotations;

namespace Blogvio.WebApi.Dtos.Blog
{
	public class BlogReadDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Url { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }
	}
}
