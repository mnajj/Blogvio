using MediatR;

namespace Blogvio.WebApi.Commands;

public record DeleteBlogCommand(int Id) : IRequest<bool>;
