using Blogvio.WebApi.Data;
using Blogvio.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Blogvio.WebApi.Repositories
{
	public class BlogRepository : IBlogRepository
	{
		private readonly AppDbContext _context;

		public BlogRepository(AppDbContext context)
		{
			_context = context;
		}

		public void CreateBlogAsync(Blog blog)
		{
			_context.Blogs.AddAsync(blog);
		}

		public async Task DeleteBlog(int id)
		{
			Blog blog = new Blog() { Id = id };
			_context.Blogs.Attach(blog);
			_context.Blogs.Remove(blog);
			await Task.CompletedTask;
		}

		public async Task<Blog> GetBlogAsync(int id)
		{
			var blog = await _context.Blogs.Where(b => b.Id == id).FirstOrDefaultAsync();
			return blog;
		}

		public async Task<IEnumerable<Blog>> GetBlogsAsync()
		{
			var blogs = await _context.Blogs.ToListAsync();
			return blogs;
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public async Task UpdateBlog(Blog blog)
		{
			_context.Entry(
				await _context.Blogs
					.FirstOrDefaultAsync(b => b.Id == blog.Id))
				.CurrentValues
				.SetValues(blog);
			await Task.CompletedTask;
		}
	}
}
