using AutoMapper;
using Blogvio.WebApi.Contracts.V1;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers
{
	[Route(ApiRoutesV1.Post.PostBase)]
	[ApiController]
	public class PostController : ControllerBase
	{
		private readonly IPostRepository _repository;
		private readonly IMapper _mapper;

		public PostController(IPostRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<PostReadDto>>> GetPostsForBlogAsync(int blogId)
		{
			var posts = await _repository.GetPostsForBlogAsync(blogId);
			return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
		}

		[HttpGet(ApiRoutesV1.Post.GetPostAsync, Name = "GetPostAsync")]
		public async Task<ActionResult<PostReadDto>> GetPostAsync(int blogId, int postId)
		{
			if (!await _repository.BlogExist(blogId))
			{
				return NotFound();
			}
			var post = await _repository.GetPostAsync(blogId, postId);
			if (post is null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<PostReadDto>(post));
		}

		[HttpPost]
		public async Task<ActionResult<PostReadDto>> CreatePostAsync(int blogId, PostCreateDto postCreateDto)
		{
			if (!await _repository.BlogExist(blogId))
			{
				return NotFound();
			}
			var post = _mapper.Map<Post>(postCreateDto);
			await _repository.CreatePostAsync(blogId, post);
			if (!await _repository.SaveChangesAsync())
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			var postReadDto = _mapper.Map<PostReadDto>(post);
			return CreatedAtRoute(nameof(GetPostAsync), new { blogId = blogId, postId = postReadDto.Id }, postReadDto);
		}

		[HttpPut(ApiRoutesV1.Post.UpdatePostAsync)]
		public async Task<ActionResult> UpdatePostAsync(int blogId, int postId, PostUpdateDto postUpdateDto)
		{
			if (!await _repository.BlogExist(blogId) ||
						await _repository.GetPostAsync(blogId, postId) is null)
			{
				return NotFound();
			}
			var post = _mapper.Map<Post>(postUpdateDto);
			post.Id = postId;
			await _repository.UpdatePostAsync(blogId, post);
			if (!await _repository.SaveChangesAsync())
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			return NoContent();
		}

		[HttpDelete(ApiRoutesV1.Post.DeletePostAsync)]
		public async Task<ActionResult> DeltePostAsync(int blogId, int postId)
		{
			if (!await _repository.BlogExist(blogId))
			{
				return NotFound();
			}
			if (await _repository.GetPostAsync(blogId, postId) is null)
			{
				return NotFound();
			}
			await _repository.DeletePostAsync(postId);
			if (!await _repository.SaveChangesAsync())
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			return NoContent();
		}

		[HttpGet(ApiRoutesV1.Post.SearchForPostAsync, Name = "SearchForPostAsync")]
		public async Task<ActionResult<IEnumerable<PostReadDto>>> SearchForPostAsync(int blogId, string keyword)
		{
			if (!await _repository.BlogExist(blogId))
			{
				return NotFound();
			}
			var results = await _repository.SearchForPostAsync(keyword);
			var resReadDto = _mapper.Map<IEnumerable<PostReadDto>>(results);
			return Ok(resReadDto);
		}
	}
}
