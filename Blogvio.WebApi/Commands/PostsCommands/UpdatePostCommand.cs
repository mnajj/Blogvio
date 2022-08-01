using Blogvio.WebApi.Dtos.Post;
using MediatR;

namespace Blogvio.WebApi.Commands.PostsCommands;

public record UpdatePostCommand(int BlogId, int PostId, PostUpdateDto PostUpdateDto)
	: IRequest<bool>;
