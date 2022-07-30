using Blogvio.WebApi.Infrastructure.Services;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories.IRepository;
using System.Text;
using System.Text.Json;

namespace Blogvio.WebApi.Repositories.CachedRepository;

public class CachedBlogRepository : IBlogRepository
{
	private readonly IBlogRepository _blogRepository;
	private readonly ICacheService _cacheService;

	public CachedBlogRepository(IBlogRepository blogRepository, ICacheService cacheService)
	{
		_blogRepository = blogRepository;
		_cacheService = cacheService;
	}

	public async Task CreateBlogAsync(Blog blog)
	{
		await _blogRepository.CreateBlogAsync(blog);
		await _cacheService.ClearCachedPagesAsync();
	}

	public async Task DeleteBlogAsync(int id)
	{
		await _blogRepository.DeleteBlogAsync(id);
		await _cacheService.ClearCachedPagesAsync();
	}

	public async Task<Blog> GetBlogAsync(int id)
	{
		var blogJson = await _cacheService.GetCachedValueAsync($"blog:{id}");
		if (blogJson != null)
		{
			return JsonSerializer.Deserialize<Blog>(blogJson);
		}
		var blog = await _blogRepository.GetBlogAsync(id);
		await _cacheService.SetCacheValueAsync(
			$"blog:{id}",
			JsonSerializer.Serialize(blog)
			);
		return blog;
	}

	public async Task<IEnumerable<Blog>> GetBlogsAsync(PaginationFilter paginationFilter)
	{
		var cached = await _cacheService.GetCachedValueAsync($"page:{paginationFilter.PageNumber}");
		if (cached != null)
		{
			var page = await _cacheService.GetCachedPageAsync($"page:{paginationFilter.PageNumber}");
			return JsonSerializer.Deserialize<IEnumerable<Blog>>(page);
		}

		var blogs = await _blogRepository.GetBlogsAsync(paginationFilter);
		try
		{
			StringBuilder pageKeys = new StringBuilder();
			foreach (var blog in blogs)
			{
				await _cacheService.SetCacheValueAsync(
					$"blog:{blog.Id}",
					JsonSerializer.Serialize(blog));
				pageKeys.Append($"blog:{blog.Id},");
			}
			await _cacheService.SetCacheValueAsync(
				$"page:{paginationFilter.PageNumber}",
				pageKeys.ToString().TrimEnd(','));
		}
		catch (Exception ex)
		{

		}
		return blogs;
	}

	public async Task<bool> SaveChangesAsync()
	{
		return await _blogRepository.SaveChangesAsync();
	}

	public async Task UpdateBlogAsync(Blog blog)
	{
		await _blogRepository.UpdateBlogAsync(blog);
		await _cacheService.SetCacheValueAsync($"blog:{blog.Id}", JsonSerializer.Serialize(blog));
		await Task.CompletedTask;
	}
}
