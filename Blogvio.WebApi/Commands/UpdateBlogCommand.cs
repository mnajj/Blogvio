using Blogvio.WebApi.Dtos.Blog;
using MediatR;

namespace Blogvio.WebApi.Commands;

public record UpdateBlogCommand(int Id, BlogUpdateDto UpdateDto) : IRequest<bool>;
