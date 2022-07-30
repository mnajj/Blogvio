namespace Blogvio.WebApi.Infrastructure.Services;

public interface ICacheService
{
	Task<string?> GetCachedValueByPatternAsync(string key);
	Task<string?> GetCachedValueAsync(string key);
	Task SetCacheValueAsync(string key, string value);
	Task<string> GetCachedPageAsync(string page);
	Task ClearCachedPagesAsync();
}
