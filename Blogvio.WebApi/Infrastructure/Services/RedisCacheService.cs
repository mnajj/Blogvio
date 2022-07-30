using StackExchange.Redis;
using System.Text;

namespace Blogvio.WebApi.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
	private readonly IConnectionMultiplexer _connectionMultiplexer;
	private readonly IDatabase _db;
	private readonly IServer _server;

	public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
	{
		_connectionMultiplexer = connectionMultiplexer;
		_db = connectionMultiplexer.GetDatabase();
		_server = _connectionMultiplexer.GetServer(
			"redis-11459.c293.eu-central-1-1.ec2.cloud.redislabs.com",
			11459, asyncState: null);
	}

	public async Task ClearCachedPagesAsync()
	{
		var keys = await _server.KeysAsync(pattern: "page:*").ToArrayAsync();
		await _db.KeyDeleteAsync(keys);
	}

	public async Task<string> GetCachedPageAsync(string page)
	{
		var p = await _db.StringGetAsync(page);
		var pageElements = p.ToString().TrimEnd(',').Split(',');
		var keysArray = new RedisKey[pageElements.Length];
		for (var i = 0; i < pageElements.Length; i++)
		{
			keysArray[i] = new RedisKey(pageElements[i]);
		}
		var values = await _db.StringGetAsync(keysArray);
		var res = CreateJsonResult(values);
		return res;
	}

	public async Task<string?> GetCachedValueAsync(string key)
	{
		return await _db.StringGetAsync(key);
	}

	public async Task<string?> GetCachedValueByPatternAsync(string key)
	{
		var keys = await _server.KeysAsync(pattern: key).ToArrayAsync();
		if (!keys.Any())
		{
			return null;
		}

		var values = await _db.StringGetAsync(keys);
		return CreateJsonResult(values);
	}

	public async Task SetCacheValueAsync(string key, string value)
	{
		await _db.StringSetAsync(key, value);
	}


	private string CreateJsonResult(RedisValue[] values)
	{
		var jsonResult = new StringBuilder();
		jsonResult.Clear();
		jsonResult.Append("[");
		foreach (var value in values)
		{
			jsonResult.Append(value.ToString());
			jsonResult.Append(",");
		}
		var jsonString = jsonResult.ToString().TrimEnd(',');
		jsonString += ']';
		return jsonString;
	}
}
