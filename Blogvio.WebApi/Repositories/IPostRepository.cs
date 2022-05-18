using Blogvio.WebApi.Models;

namespace Blogvio.WebApi.Repositories
{
	public interface IPostRepository
	{
		Task<IEnumerable<Post>> GetPostsForBlogAsync(int blogId);
		Task<Post> GetPostAsync(int blogId, int postId);
		Task CreatePostAsync(int blogId, Post post);
		Task UpdatePostAsync(int blogId, Post post);
		Task DeletePostAsync(int id);
		Task<bool> BlogExist(int blogId);
		Task<bool> SaveChanges();
	}
}
