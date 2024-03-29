﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogvio.WebApi.Models
{
	public class Post
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[MaxLength(200)]
		public string Content { get; set; }

		[Required]
		public DateTime PublishedAt { get; set; } = DateTime.Now;

		public DateTime? UpdatedAt { get; set; }

		[Required]
		public bool IsDeleted { get; set; }

		[ForeignKey("Blog")]
		public int BlogId { get; set; }

		public Blog Blog { get; set; }
		public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
	}
}
