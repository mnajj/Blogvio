using AutoMapper;
using Blogvio.WebApi.Commands;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Response;
using MediatR;

namespace Blogvio.WebApi.Handlers;

public class CreateBlogHandler : IRequestHandler<CreateBlogCommand, Response<BlogReadDto>>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;
	private object blogDto;

	public CreateBlogHandler(IMapper mapper, IBlogRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<Response<BlogReadDto>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
	{
		var blogModel = _mapper.Map<Blog>(request.BlogCreateDto);
		await _repository.CreateBlogAsync(blogModel);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		return new Response<BlogReadDto>(_mapper.Map<BlogReadDto>(blogModel));
	}
}
