using Blogvio.WebApi.Models;

namespace Blogvio.WebApi.Repositories.IRepository
{
	public interface IBlogRepository
	{
		Task<IEnumerable<Blog>> GetBlogsAsync(PaginationFilter? paginationFilter);
		Task<Blog> GetBlogAsync(int id);
		Task CreateBlogAsync(Blog blog);
		Task UpdateBlogAsync(Blog blog);
		Task DeleteBlogAsync(int id);
		Task<bool> SaveChangesAsync();
	}
}
