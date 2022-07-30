using AutoMapper;
using Blogvio.WebApi.Contracts.V1;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Helpers;
using Blogvio.WebApi.Infrastructure.Exceptions;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers.V1;

[Route(ApiRoutesV1.Post.PostBase)]
[ApiController]
public class PostController : ControllerBase
{
	private readonly IPostRepository _repository;
	private readonly IMapper _mapper;
	private readonly IUriService _uriService;

	public PostController(
		IPostRepository repository,
		IMapper mapper,
		IUriService uriService)
	{
		_repository = repository;
		_mapper = mapper;
		_uriService = uriService;
	}

	[HttpGet]
	public async Task<IActionResult> GetPostsForBlogAsync(
		int blogId, [FromQuery] PaginationQuery paginationQuery)
	{
		if (!await _repository.IsBlogExist(blogId))
		{
			throw new EntityNotFoundException();
		}
		var pagingFilter = _mapper.Map<PaginationFilter>(paginationQuery);
		var posts = await _repository.GetPostsForBlogAsync(blogId, pagingFilter);
		var postsResponseContent = _mapper.Map<IEnumerable<PostReadDto>>(posts);
		if (pagingFilter is null ||
				pagingFilter.PageNumber < 1 ||
				pagingFilter.PageSize < 1)
		{
			return Ok(new PageResponse<PostReadDto>(postsResponseContent));
		}
		var pagingResponse = PaginationHelper.CreatePaginationResponse(
			_uriService, pagingFilter, postsResponseContent);
		return Ok(pagingResponse);
	}

	[HttpGet(ApiRoutesV1.Post.GetPostAsync, Name = "GetPostAsync")]
	public async Task<IActionResult> GetPostAsync(int blogId, int postId)
	{
		if (!await _repository.IsBlogExist(blogId))
		{
			throw new EntityNotFoundException();
		}
		var post = await _repository.GetPostAsync(blogId, postId);
		if (post is null)
		{
			throw new EntityNotFoundException();
		}
		return Ok(new Response<PostReadDto>(_mapper.Map<PostReadDto>(post)));
	}

	[HttpPost]
	public async Task<ActionResult<PostReadDto>> CreatePostAsync(int blogId, PostCreateDto postCreateDto)
	{
		if (!await _repository.IsBlogExist(blogId))
		{
			throw new EntityNotFoundException();
		}
		var post = _mapper.Map<Post>(postCreateDto);
		await _repository.CreatePostAsync(blogId, post);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		var postReadDto = _mapper.Map<PostReadDto>(post);
		return CreatedAtRoute(
			nameof(GetPostAsync),
			new { blogId = blogId, postId = postReadDto.Id },
			new Response<PostReadDto>(postReadDto));
	}

	[HttpPut(ApiRoutesV1.Post.UpdatePostAsync)]
	public async Task<ActionResult> UpdatePostAsync(int blogId, int postId, PostUpdateDto postUpdateDto)
	{
		if (!await _repository.IsBlogExist(blogId) ||
					await _repository.GetPostAsync(blogId, postId) is null)
		{
			throw new EntityNotFoundException();
		}
		var post = _mapper.Map<Post>(postUpdateDto);
		post.Id = postId;
		await _repository.UpdatePostAsync(blogId, post);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		return NoContent();
	}

	[HttpDelete(ApiRoutesV1.Post.DeletePostAsync)]
	public async Task<ActionResult> DeltePostAsync(int blogId, int postId)
	{
		if (!await _repository.IsBlogExist(blogId)
			|| await _repository.GetPostAsync(blogId, postId) is null)
		{
			throw new EntityNotFoundException();
		}
		await _repository.DeletePostAsync(postId);
		if (!await _repository.SaveChangesAsync())
		{
			throw new DbCommitFailException();
		}
		return NoContent();
	}

	[HttpGet(ApiRoutesV1.Post.SearchForPostAsync, Name = "SearchForPostAsync")]
	public async Task<ActionResult<IEnumerable<PostReadDto>>> SearchForPostAsync(int blogId, string keyword)
	{
		if (!await _repository.IsBlogExist(blogId))
		{
			throw new EntityNotFoundException();
		}
		var results = await _repository.SearchForPostAsync(keyword);
		var resReadDto = _mapper.Map<IEnumerable<PostReadDto>>(results);
		return Ok(resReadDto);
	}
}
