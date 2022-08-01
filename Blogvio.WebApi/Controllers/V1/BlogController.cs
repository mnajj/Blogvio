using Blogvio.WebApi.Commands;
using Blogvio.WebApi.Contracts.V1;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Models.Response;
using Blogvio.WebApi.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers.V1;

[Route(ApiRoutesV1.Blogs.BlogBase)]
[ApiController]
[Produces("application/json")]
public class BlogController : ControllerBase
{
	private readonly IMediator _mediator;

	public BlogController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>Get all blogs</summary>
	[HttpGet]
	[ProducesResponseType(typeof(PageResponse<BlogReadDto>), 200)]
	public async Task<IActionResult> GetBlogsAsync([FromQuery] PaginationQuery paginationQuery)
	{
		var result = await _mediator.Send(new GetBlogsQuery(paginationQuery));
		return Ok(result);
	}

	/// <summary>Get blog by id</summary>
	[HttpGet(ApiRoutesV1.Blogs.GetBlogByIdAsync, Name = "GetBlogByIdAsync")]
	[ProducesResponseType(typeof(Response<BlogReadDto>), 200)]
	[ProducesResponseType(typeof(ProblemDetails), 404)]
	public async Task<ActionResult> GetBlogByIdAsync(int id)
	{
		var result = await _mediator.Send(
			new GetBlogByIdQuery(id));
		return Ok(result);
	}

	/// <summary>Create new blog</summary>
	[HttpPost]
	[ProducesResponseType(typeof(Response<BlogReadDto>), 201)]
	[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
	[ProducesResponseType(typeof(ProblemDetails), 500)]
	public async Task<ActionResult> CreateBlogAsync(BlogCreateDto blogDto)
	{
		var result = await _mediator.Send(new CreateBlogCommand(blogDto));
		return CreatedAtRoute(nameof(GetBlogByIdAsync), new { Id = result.Data.Id }, result);
	}

	/// <summary>Update blog by id</summary>
	[HttpPut(ApiRoutesV1.Blogs.UpdateBlogAsync)]
	public async Task<ActionResult> UpdateBlogAsync(int id, BlogUpdateDto updateDto)
	{
		await _mediator.Send(new UpdateBlogCommand(id, updateDto));
		return NoContent();
	}

	/// <summary>Delete blog by id</summary>
	[HttpDelete(ApiRoutesV1.Blogs.DeleteBlogAsync)]
	public async Task<ActionResult> DeleteBlogAsync(int id)
	{
		await _mediator.Send(new DeleteBlogCommand(id));
		return NoContent();
	}
}