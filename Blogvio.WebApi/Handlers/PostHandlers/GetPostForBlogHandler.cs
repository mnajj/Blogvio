using AutoMapper;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models.Response;
using Blogvio.WebApi.Queries.PostQueries;
using MediatR;

namespace Blogvio.WebApi.Handlers.PostHandlers;

public class GetPostForBlogHandler : IRequestHandler<GetPostForBlogQuery, Response<PostReadDto>>
{
	private readonly IPostRepository _repository;
	private readonly IMapper _mapper;

	public GetPostForBlogHandler(IMapper mapper, IPostRepository repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<Response<PostReadDto>> Handle(GetPostForBlogQuery request, CancellationToken cancellationToken)
	{
		if (!await _repository.IsBlogExist(request.BlogId))
		{
			throw new EntityNotFoundException();
		}
		var post = await _repository.GetPostAsync(request.BlogId, request.PostId);
		if (post is null)
		{
			throw new EntityNotFoundException();
		}
		return new Response<PostReadDto>(_mapper.Map<PostReadDto>(post));
	}
}
