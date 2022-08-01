using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Models.Response;
using MediatR;

namespace Blogvio.WebApi.Queries;

public record GetBlogsQuery(PaginationQuery PaginationQuery) : IRequest<PageResponse<BlogReadDto>>;
