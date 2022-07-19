using Blogvio.WebApi.Data;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Blogvio.WebApi.Repositories.Repository.SQLServer
{
	public class BlogRepository : IBlogRepository
	{
		private readonly AppDbContext _context;

		public BlogRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task CreateBlogAsync(Blog blog)
		{
			await _context.Blogs.AddAsync(blog);
		}

		public async Task DeleteBlogAsync(int id)
		{
			var blog = await GetBlogAsync(id);
			blog.IsDeleted = true;
			await Task.CompletedTask;
		}

		public async Task<Blog> GetBlogAsync(int id)
		{
			return await _context.Blogs
				.Where(b => b.Id == id &&
				!b.IsDeleted)
				.FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Blog>> GetBlogsAsync(PaginationFilter? paginationFilter)
		{
			if (paginationFilter == null)
			{
				return await _context.Blogs
					.Include(t => t.Posts)
					.Where(b => !b.IsDeleted)
					.ToListAsync();
			}
			var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
			return await _context.Blogs
				.Include(t => t.Posts)
				.Where(b => !b.IsDeleted)
				.Skip(skip)
				.Take(paginationFilter.PageSize)
				.ToListAsync();
		}

		public async Task<bool> SaveChangesAsync()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public async Task UpdateBlogAsync(Blog blog)
		{
			_context.Entry(
				await _context.Blogs
					.FirstOrDefaultAsync(b => b.Id == blog.Id))
				.CurrentValues
				.SetValues(blog);
		}
	}
}
