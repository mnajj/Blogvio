using Blogvio.WebApi.Models;

namespace Blogvio.WebApi.Repositories
{
	public interface IBlogRepository
	{
		Task<IEnumerable<Blog>> GetBlogsAsync();
		Task<Blog> GetBlogAsync(int id);
		Task CreateBlogAsync(Blog blog);
		Task UpdateBlog(Blog blog);
		Task DeleteBlog(int id);
		Task<bool> SaveChanges();
	}
}
