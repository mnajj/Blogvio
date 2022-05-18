using Blogvio.WebApi.Models;

namespace Blogvio.WebApi.Repositories
{
	public interface IPostRepository
	{
		Task<IEnumerable<Post>> GetPostsAsync();
		Task<Post> GetPostByIdAsync(int id);
		Task CreatePostAsync(Post post);
		Task UpdatePostAsync(Post post);
		Task DeletePostAsync(int id);
		Task<bool> SaveChanges();
	}
}
