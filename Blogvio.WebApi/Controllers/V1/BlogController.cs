using AutoMapper;
using Blogvio.WebApi.Contracts.V1;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Helpers;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers.V1;

[Route(ApiRoutesV1.Blogs.BlogBase)]
[ApiController]
[Produces("application/json")]
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

	/// <summary>Get all blogs</summary>
	[HttpGet]
	[ProducesResponseType(typeof(PageResponse<BlogReadDto>), 200)]
	public async Task<IActionResult> GetBlogsAsync([FromQuery] PaginationQuery paginationQuery)
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

		var paginationResponse = PaginationHelper
			.CreatePaginationResponse(_uriService, paginationFilter, blogsResponse);
		return Ok(paginationResponse);
	}

	/// <summary>Get blog by id</summary>
	[HttpGet(ApiRoutesV1.Blogs.GetBlogByIdAsync, Name = "GetBlogByIdAsync")]
	[ProducesResponseType(typeof(Response<BlogReadDto>), 200)]
	[ProducesResponseType(typeof(ProblemDetails), 404)]
	public async Task<ActionResult> GetBlogByIdAsync(int id)
	{
		var blog = await _repository.GetBlogAsync(id);
		if (blog is null)
		{
			throw new EntityNotFoundException();
		}
		var response = new Response<BlogReadDto>(
			_mapper.Map<BlogReadDto>(blog));
		return Ok(response);
	}

	/// <summary>Create new blog</summary>
	[HttpPost]
	[ProducesResponseType(typeof(Response<BlogReadDto>), 201)]
	[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
	[ProducesResponseType(typeof(ProblemDetails), 500)]
	public async Task<ActionResult> CreateBlogAsync(BlogCreateDto blogDto)
	{
		var blogModel = _mapper.Map<Blog>(blogDto);
		await _repository.CreateBlogAsync(blogModel);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}

		var response = new Response<BlogReadDto>(_mapper.Map<BlogReadDto>(blogModel));
		return CreatedAtRoute(nameof(GetBlogByIdAsync), new { Id = blogModel.Id }, response);
	}

	/// <summary>Update blog by id</summary>
	[HttpPut(ApiRoutesV1.Blogs.UpdateBlogAsync)]
	public async Task<ActionResult> UpdateBlogAsync(int id, BlogUpdateDto updateDto)
	{
		var existingBlog = await _repository.GetBlogAsync(id);
		if (existingBlog is null)
		{
			throw new EntityNotFoundException();
		}

		var blogModel = _mapper.Map<Blog>(updateDto);
		blogModel.Id = id;
		await _repository.UpdateBlogAsync(blogModel);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}

		return NoContent();
	}

	/// <summary>Delete blog by id</summary>
	[HttpDelete(ApiRoutesV1.Blogs.DeleteBlogAsync)]
	public async Task<ActionResult> DeleteBlogAsync(int id)
	{
		var existningBlog = await _repository.GetBlogAsync(id);
		if (existningBlog is null)
		{
			throw new EntityNotFoundException();
		}

		await _repository.DeleteBlogAsync(id);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}

		return NoContent();
	}
}