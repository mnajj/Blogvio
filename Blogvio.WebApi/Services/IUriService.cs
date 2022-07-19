using Blogvio.WebApi.Dtos.Queries;

namespace Blogvio.WebApi.Services;

public interface IUriService
{
	Uri GetAllBlogsUri(PaginationQuery? paginationQuery);
}