using AutoMapper;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Helpers;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Response;
using Blogvio.WebApi.Queries;
using MediatR;

namespace Blogvio.WebApi.Handlers;

public class GetBlogsHandler : IRequestHandler<GetBlogsQuery, PageResponse<BlogReadDto>>
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;
	private readonly IUriService _uriService;

	public GetBlogsHandler(IBlogRepository repository, IMapper mapper, IUriService uriService)
	{
		_repository = repository;
		_mapper = mapper;
		_uriService = uriService;
	}

	public async Task<PageResponse<BlogReadDto>> Handle(GetBlogsQuery request, CancellationToken cancellationToken)
	{
		var paginationFilter = _mapper.Map<PaginationFilter>(request.PaginationQuery);
		var blogs = await _repository.GetBlogsAsync(paginationFilter);

		var blogsResponse = _mapper.Map<IEnumerable<BlogReadDto>>(blogs);

		if (paginationFilter == null ||
				paginationFilter.PageNumber < 1 ||
				paginationFilter.PageSize < 1)
		{
			return new PageResponse<BlogReadDto>(blogsResponse);
		}

		return PaginationHelper.CreatePaginationResponse(
			_uriService, paginationFilter, blogsResponse);
	}
}
