using AutoMapper;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Helpers;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Response;
using Blogvio.WebApi.Queries.PostQueries;
using MediatR;

namespace Blogvio.WebApi.Handlers.PostHandlers;

public class GetPostsForBlogHandler : IRequestHandler<GetPostsForBlogQuery, PageResponse<PostReadDto>>
{
	private readonly IPostRepository _repository;
	private readonly IMapper _mapper;
	private readonly IUriService _uriService;

	public GetPostsForBlogHandler(IPostRepository repository, IMapper mapper, IUriService uriService)
	{
		_repository = repository;
		_mapper = mapper;
		_uriService = uriService;
	}

	public async Task<PageResponse<PostReadDto>> Handle(GetPostsForBlogQuery request, CancellationToken cancellationToken)
	{
		if (!await _repository.IsBlogExist(request.BlogId))
		{
			throw new EntityNotFoundException();
		}
		var pagingFilter = _mapper.Map<PaginationFilter>(request.PaginationQuery);
		var posts = await _repository.GetPostsForBlogAsync(request.BlogId, pagingFilter);
		var postsResponseContent = _mapper.Map<IEnumerable<PostReadDto>>(posts);
		if (pagingFilter is null ||
				pagingFilter.PageNumber < 1 ||
				pagingFilter.PageSize < 1)
		{
			return new PageResponse<PostReadDto>(postsResponseContent);
		}
		return PaginationHelper.CreatePaginationResponse(
			_uriService, pagingFilter, postsResponseContent);
	}
}
