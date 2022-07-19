using AutoMapper;
using Blogvio.WebApi.Contracts.V1;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Helpers;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Response;
using Blogvio.WebApi.Repositories.IRepository;
using Blogvio.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers.V1;

[Route(ApiRoutesV1.Blogs.BlogBase)]
[ApiController]
public class BlogController : ControllerBase
{
	private readonly IBlogRepository _repository;
	private readonly IMapper _mapper;
	private readonly IUriService _uriService;

	public BlogController(
		IBlogRepository repository,
		IMapper mapper,
		IUriService uriService
	)
	{
		_repository = repository;
		_mapper = mapper;
		_uriService = uriService;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<BlogReadDto>>> GetBlogsAsync([FromQuery] PaginationQuery paginationQuery)
	{
		var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
		var blogs = await _repository.GetBlogsAsync(paginationFilter);
		var blogsResponse = _mapper.Map<IEnumerable<BlogReadDto>>(blogs);

		if (paginationFilter == null ||
		    paginationFilter.PageNumber < 1 ||
		    paginationFilter.PageSize < 1)
		{
			return Ok(new PageResponse<BlogReadDto>(blogsResponse));
		}

		var paginationResponse = PaginationHelper.CreatePaginationResponse(
			_uriService, paginationFilter, blogsResponse);
		return Ok(paginationResponse);
	}

	[HttpGet(ApiRoutesV1.Blogs.GetBlogByIdAsync, Name = "GetBlogByIdAsync")]
	public async Task<ActionResult<BlogReadDto>> GetBlogByIdAsync(int id)
	{
		var blog = await _repository.GetBlogAsync(id);
		if (blog is null)
		{
			return NotFound();
		}

		var response = new Response<BlogReadDto>(
			_mapper.Map<BlogReadDto>(blog));
		return Ok(response);
	}

	[HttpPost]
	public async Task<ActionResult<BlogReadDto>> CreateBlogAsync(BlogCreateDto blogDto)
	{
		var blogModel = _mapper.Map<Blog>(blogDto);
		await _repository.CreateBlogAsync(blogModel);
		if (!await _repository.SaveChangesAsync())
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		var response = new Response<BlogReadDto>(_mapper.Map<BlogReadDto>(blogModel));
		return CreatedAtRoute(nameof(GetBlogByIdAsync), new { Id = blogModel.Id }, response);
	}

	[HttpPut(ApiRoutesV1.Blogs.UpdateBlogAsync)]
	public async Task<ActionResult> UpdateBlogAsync(int id, BlogUpdateDto updateDto)
	{
		var existingBlog = await _repository.GetBlogAsync(id);
		if (existingBlog is null)
		{
			return NotFound();
		}

		var blogModel = _mapper.Map<Blog>(updateDto);
		await _repository.UpdateBlogAsync(blogModel);
		if (!await _repository.SaveChangesAsync())
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}

	[HttpDelete(ApiRoutesV1.Blogs.DeleteBlogAsync)]
	public async Task<ActionResult> DeleteBlogAsync(int id)
	{
		var existningBlog = await _repository.GetBlogAsync(id);
		if (existningBlog is null)
		{
			return NotFound();
		}

		await _repository.DeleteBlogAsync(id);
		if (!await _repository.SaveChangesAsync())
		{
			return StatusCode(StatusCodes.Status500InternalServerError);
		}

		return NoContent();
	}
}