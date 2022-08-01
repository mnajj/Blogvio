using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Models.Response;
using MediatR;

namespace Blogvio.WebApi.Commands.PostsCommands;

public record CreatePostCommand(int BlogId, PostCreateDto PostCreateDto) :
	IRequest<Response<PostReadDto>>;

