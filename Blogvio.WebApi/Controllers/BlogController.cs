using AutoMapper;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogController : ControllerBase
	{
		private readonly IBlogRepository _repository;
		private readonly IMapper _mapper;

		public BlogController(IBlogRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet("{id}", Name = "GetBlogById")]
		public async Task<ActionResult<BlogReadDto>> GetBlogById(int id)
		{
			var blog = await _repository.GetBlogAsync(id);
			if (blog is not null)
			{
				var blogReadDto = _mapper.Map<BlogReadDto>(blog);
				return Ok(blogReadDto);
			}
			return NotFound();
		}
	}
}
