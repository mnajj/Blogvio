using AutoMapper;
using Blogvio.WebApi.Commands;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using MediatR;

namespace Blogvio.WebApi.Handlers;

public class DeleteBlogHandler : IRequestHandler<DeleteBlogCommand, bool>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;

	public DeleteBlogHandler(IMapper mapper, IBlogRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<bool> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
	{
		var existningBlog = await _repository.GetBlogAsync(request.Id);
		if (existningBlog is null)
		{
			throw new EntityNotFoundException();
		}
		await _repository.DeleteBlogAsync(request.Id);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		return true;
	}
}
