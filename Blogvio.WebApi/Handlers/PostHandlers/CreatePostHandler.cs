using AutoMapper;
using Blogvio.WebApi.Commands.PostsCommands;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Response;
using MediatR;

namespace Blogvio.WebApi.Handlers.PostHandlers;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, Response<PostReadDto>>
{
	private readonly IPostRepository _repository;
	private readonly IMapper _mapper;

	public CreatePostHandler(IMapper mapper, IPostRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<Response<PostReadDto>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
	{
		if (!await _repository.IsBlogExist(request.BlogId))
		{
			throw new EntityNotFoundException();
		}
		var post = _mapper.Map<Post>(request.PostCreateDto);
		await _repository.CreatePostAsync(request.BlogId, post);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		return new Response<PostReadDto>(_mapper.Map<PostReadDto>(post));
	}
}
