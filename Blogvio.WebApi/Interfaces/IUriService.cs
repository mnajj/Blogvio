using Blogvio.WebApi.Dtos.Queries;

namespace Blogvio.WebApi.Interfaces;

public interface IUriService
{
	Uri GetAllBlogsUri(PaginationQuery? paginationQuery);
}