using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Models.Response;
using MediatR;

namespace Blogvio.WebApi.Queries.PostQueries;

public record GetPostsForBlogQuery(int BlogId, PaginationQuery PaginationQuery) :
	IRequest<PageResponse<PostReadDto>>;
