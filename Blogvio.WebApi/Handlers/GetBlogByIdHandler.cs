using AutoMapper;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models.Response;
using Blogvio.WebApi.Queries;
using MediatR;

namespace Blogvio.WebApi.Handlers;

public class GetBlogByIdHandler : IRequestHandler<GetBlogByIdQuery, Response<BlogReadDto>>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;

	public GetBlogByIdHandler(IMapper mapper, IBlogRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<Response<BlogReadDto>> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
	{
		var blog = await _repository.GetBlogAsync(request.Id);
		if (blog is null)
		{
			throw new EntityNotFoundException();
		}
		return new Response<BlogReadDto>(_mapper.Map<BlogReadDto>(blog));
	}
}
