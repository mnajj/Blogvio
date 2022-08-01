using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Models.Response;
using MediatR;

namespace Blogvio.WebApi.Queries;

public record GetBlogByIdQuery(int Id) : IRequest<Response<BlogReadDto>>;
