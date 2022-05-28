using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogvio.WebApi.Models.Identity
{
	public class RefreshToken
	{
		[Key]
		public string Token { get; set; }
		public string JwtId { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime ExpiresOn { get; set; }
		public bool IsUsed { get; set; }
		public bool IsInvalidated { get; set; }
		public string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public AppUser User { get; set; }
	}
}
