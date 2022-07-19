using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blogvio.WebApi.Models.SQLServerModels
{
	public class AppUser : IdentityUser
	{
		[Required, MaxLength(50)]
		public string FirstName { get; set; }

		[Required, MaxLength(50)]
		public string LastName { get; set; }
	}
}
