using Blogvio.WebApi.Dtos.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace Blogvio.WebApi.Infrastructure.Services;

public class UriService : IUriService
{
	private readonly string _baseUri;

	public UriService(string baseUri)
	{
		_baseUri = baseUri;
	}

	public Uri GetAllBlogsUri(PaginationQuery? paginationQuery)
	{
		var uri = new Uri(_baseUri);
		if (paginationQuery == null)
		{
			return uri;
		}

		var modifiedUri = QueryHelpers.AddQueryString(
			_baseUri,
			"pageNumber",
			paginationQuery.PageNumber.ToString());
		modifiedUri = QueryHelpers.AddQueryString(
			modifiedUri,
			"pageSize",
			paginationQuery.PageSize.ToString());

		return new Uri(modifiedUri);
	}
}