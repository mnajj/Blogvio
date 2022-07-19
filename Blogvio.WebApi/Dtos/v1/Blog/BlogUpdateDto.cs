using System.ComponentModel.DataAnnotations;

namespace Blogvio.WebApi.Dtos.Blog
{
	public class BlogUpdateDto
	{
		[Required]
		public string Name { get; set; }
	}
}
