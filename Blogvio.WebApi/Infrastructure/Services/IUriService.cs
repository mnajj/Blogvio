using Blogvio.WebApi.Dtos.Queries;

namespace Blogvio.WebApi.Infrastructure.Services;

public interface IUriService
{
	Uri GetAllBlogsUri(PaginationQuery? paginationQuery);
}