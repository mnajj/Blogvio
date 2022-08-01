using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Models.Response;
using MediatR;

namespace Blogvio.WebApi.Commands;

public record CreateBlogCommand(BlogCreateDto BlogCreateDto) : IRequest<Response<BlogReadDto>>;
