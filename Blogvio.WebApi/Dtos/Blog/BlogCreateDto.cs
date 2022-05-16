using System.ComponentModel.DataAnnotations;

namespace Blogvio.WebApi.Dtos.Blog
{
	public class BlogCreateDto
	{
		[Required]
		public string Url { get; set; }
	}
}
