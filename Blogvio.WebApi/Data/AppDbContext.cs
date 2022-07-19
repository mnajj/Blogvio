using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Identity;
using Blogvio.WebApi.Models.SQLServerModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blogvio.WebApi.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Blog> Blogs { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
	}
}
