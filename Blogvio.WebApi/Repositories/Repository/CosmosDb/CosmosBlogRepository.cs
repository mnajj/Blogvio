using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.CosmosModels;
using Cosmonaut;
using Cosmonaut.Extensions;

namespace Blogvio.WebApi.Repositories
{
	public class CosmosBlogRepository : IBlogRepository
	{
		private readonly ICosmosStore<CosmosBlog> _cosmosStore;

		public CosmosBlogRepository(ICosmosStore<CosmosBlog> cosmosStore)
		{
			_cosmosStore = cosmosStore;
		}

		public async Task CreateBlogAsync(Blog blog)
		{
			var newBlog = new CosmosBlog()
			{
				Id = Guid.NewGuid().ToString(),
				Url = blog.Url,
				IsDeleted = blog.IsDeleted,
				CreatedAt = blog.CreatedAt,
			};
			await _cosmosStore.AddAsync(newBlog);
		}

		public Task DeleteBlogAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Blog> GetBlogAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Blog>> GetBlogsAsync()
		{
			var blogs = await _cosmosStore.Query().ToListAsync();
			return blogs.Select(b => new Blog { Url = b.Url, IsDeleted = b.IsDeleted });
		}

		public Task<bool> SaveChangesAsync()
		{
			throw new NotImplementedException();
		}

		public Task UpdateBlogAsync(Blog blog)
		{
			throw new NotImplementedException();
		}
	}
}
