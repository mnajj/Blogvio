using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogvio.WebApi.Models
{
	public class Comment
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Content { get; set; }

		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime PublishedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		[Required]
		public bool IsDeleted { get; set; }

		[ForeignKey("Post")]
		public int PostId { get; set; }

		public virtual Post Post { get; set; }
	}
}
