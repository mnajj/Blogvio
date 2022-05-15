using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogvio.WebApi.Models
{
	public class Blog
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Url { get; set; }

		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreatedAt { get; set; }

		[Required]
		public bool IsDeleted { get; set; }

		public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
	}
}
