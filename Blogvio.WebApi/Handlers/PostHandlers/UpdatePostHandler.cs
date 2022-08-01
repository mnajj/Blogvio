using AutoMapper;
using Blogvio.WebApi.Commands.PostsCommands;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using MediatR;

namespace Blogvio.WebApi.Handlers.PostHandlers;

public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, bool>
{
	private readonly IPostRepository _repository;
	private readonly IMapper _mapper;

	public UpdatePostHandler(IMapper mapper, IPostRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
	{
		if (!await _repository.IsBlogExist(request.BlogId) ||
			await _repository.GetPostAsync(request.BlogId, request.PostId) is null)
		{
			throw new EntityNotFoundException();
		}
		var post = _mapper.Map<Post>(request.PostUpdateDto);
		post.Id = request.PostId;
		await _repository.UpdatePostAsync(request.BlogId, post);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		return true;
	}
}
