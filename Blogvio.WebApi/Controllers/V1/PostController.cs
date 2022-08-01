using Blogvio.WebApi.Commands.PostsCommands;
using Blogvio.WebApi.Contracts.V1;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Queries.PostQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers.V1;

[Route(ApiRoutesV1.Post.PostBase)]
[ApiController]
public class PostController : ControllerBase
{
	private readonly IMediator _mediator;

	public PostController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IActionResult> GetPostsForBlogAsync(
		int blogId, [FromQuery] PaginationQuery paginationQuery)
	{
		return Ok(await _mediator.Send(new GetPostsForBlogQuery(blogId, paginationQuery)));
	}

	[HttpGet(ApiRoutesV1.Post.GetPostAsync, Name = "GetPostAsync")]
	public async Task<IActionResult> GetPostAsync(int blogId, int postId)
	{
		return Ok(await _mediator.Send(new GetPostForBlogQuery(blogId, postId)));
	}

	[HttpPost]
	public async Task<ActionResult<PostReadDto>> CreatePostAsync(int blogId, PostCreateDto postCreateDto)
	{
		var result = await _mediator.Send(new CreatePostCommand(blogId, postCreateDto));
		return CreatedAtRoute(
			nameof(GetPostAsync),
			new { blogId = blogId, postId = result.Data.Id },
			result);
	}

	[HttpPut(ApiRoutesV1.Post.UpdatePostAsync)]
	public async Task<ActionResult> UpdatePostAsync(int blogId, int postId, PostUpdateDto postUpdateDto)
	{
		await _mediator.Send(new UpdatePostCommand(blogId, postId, postUpdateDto));
		return NoContent();
	}

	[HttpDelete(ApiRoutesV1.Post.DeletePostAsync)]
	public async Task<ActionResult> DeletePostAsync(int blogId, int postId)
	{
		await _mediator.Send(new DeletePostCommand(blogId, postId));
		return NoContent();
	}

	//[HttpGet(ApiRoutesV1.Post.SearchForPostAsync, Name = "SearchForPostAsync")]
	//public async Task<ActionResult<IEnumerable<PostReadDto>>> SearchForPostAsync(int blogId, string keyword)
	//{
	//	if (!await _repository.IsBlogExist(blogId))
	//	{
	//		throw new EntityNotFoundException();
	//	}
	//	var results = await _repository.SearchForPostAsync(keyword);
	//	var resReadDto = _mapper.Map<IEnumerable<PostReadDto>>(results);
	//	return Ok(resReadDto);
	//}
}
