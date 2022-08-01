using AutoMapper;
using Blogvio.WebApi.Commands.PostsCommands;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using MediatR;

namespace Blogvio.WebApi.Handlers.PostHandlers;

public class DeletePostHandler : IRequestHandler<DeletePostCommand, bool>
{
	private readonly IPostRepository _repository;

	public DeletePostHandler(IMapper mapper, IPostRepository repository)
	{
		_repository = repository;
	}

	public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
	{
		if (!await _repository.IsBlogExist(request.BlogId)
			|| await _repository.GetPostAsync(request.BlogId, request.PostId) is null)
		{
			throw new EntityNotFoundException();
		}
		await _repository.DeletePostAsync(request.PostId);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		return true;
	}
}
