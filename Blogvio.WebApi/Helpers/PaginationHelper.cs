using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Infrastructure.Services;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Response;

namespace Blogvio.WebApi.Helpers;

public static class PaginationHelper
{
	public static PageResponse<T> CreatePaginationResponse<T>(IUriService uriService, PaginationFilter paginationFilter,
		IEnumerable<T> response)
	{
		var nextPage = paginationFilter.PageNumber >= 1
			? uriService.GetAllBlogsUri(
				new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString()
			: null;

		var prevPage = paginationFilter.PageNumber - 1 >= 1
			? uriService.GetAllBlogsUri(
				new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString()
			: null;

		return new PageResponse<T>
		{
			Data = response,
			PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : null,
			PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : null,
			NextPage = response.Any() ? nextPage : null,
			PreviousPage = prevPage
		};
	}
}