using AutoMapper;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers
{
	[Route("api/[controller]")]
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

		[HttpGet("{blogId}")]
		public async Task<ActionResult<IEnumerable<PostReadDto>>> GetPostsForBlogAsync(int blogId)
		{
			var posts = await _repository.GetPostsForBlogAsync(blogId);
			return Ok(_mapper.Map<IEnumerable<PostReadDto>>(posts));
		}

		[HttpGet("{blogId}/{postId}")]
		public async Task<ActionResult<PostReadDto>> GetPostAsync(int blogId, int postId)
		{
			var isBlogExist = await _repository.BlogExist(blogId);
			if (!isBlogExist)
			{
				return NotFound();
			}
			var post = await _repository.GetPostAsync(blogId, postId);
			return Ok(_mapper.Map<PostReadDto>(post));
		}
	}
}
