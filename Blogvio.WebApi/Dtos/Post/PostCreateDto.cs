using System.ComponentModel.DataAnnotations;

namespace Blogvio.WebApi.Dtos.Post
{
	public class PostCreateDto
	{
		[Required]
		[MaxLength(200)]
		public string Content { get; set; }
	}
}
