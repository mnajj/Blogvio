using Blogvio.WebApi.Models;

namespace Blogvio.WebApi.Interfaces
{
	public interface IPostRepository
	{
		Task<IEnumerable<Post>> GetPostsForBlogAsync(int blogId, PaginationFilter? paginationFilter);
		Task<Post?> GetPostAsync(int blogId, int postId);
		Task CreatePostAsync(int blogId, Post post);
		Task UpdatePostAsync(int blogId, Post post);
		Task DeletePostAsync(int id);
		Task<bool> IsBlogExist(int blogId);
		Task<bool> SaveChangesAsync();

		// ElasticSearch
		Task<IEnumerable<Post>> SearchForPostAsync(string keyword);
	}
}
