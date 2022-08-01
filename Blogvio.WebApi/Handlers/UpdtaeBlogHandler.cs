using AutoMapper;
using Blogvio.WebApi.Commands;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using MediatR;

namespace Blogvio.WebApi.Handlers;

public class UpdtaeBlogHandler : IRequestHandler<UpdateBlogCommand, bool>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;

	public UpdtaeBlogHandler(IMapper mapper, IBlogRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<bool> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
	{
		var existingBlog = await _repository.GetBlogAsync(request.Id);
		if (existingBlog is null)
		{
			throw new EntityNotFoundException();
		}

		var blogModel = _mapper.Map<Blog>(request.UpdateDto);
		blogModel.Id = request.Id;
		await _repository.UpdateBlogAsync(blogModel);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		return true;
	}
}
