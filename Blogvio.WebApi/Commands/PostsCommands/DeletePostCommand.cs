using MediatR;

namespace Blogvio.WebApi.Commands.PostsCommands;

public record DeletePostCommand(int BlogId, int PostId) : IRequest<bool>;
