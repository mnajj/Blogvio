using AutoMapper;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Models;
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

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BlogReadDto>>> GetBlogs()
		{
			var blogs = await _repository.GetBlogsAsync();
			return Ok(_mapper.Map<IEnumerable<BlogReadDto>>(blogs));
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

		[HttpPost]
		public async Task<ActionResult<BlogReadDto>> CreateBlog(BlogCreateDto blogDto)
		{
			var blogModel = _mapper.Map<Blog>(blogDto);
			await _repository.CreateBlogAsync(blogModel);
			await _repository.SaveChanges();
			var blogRead = _mapper.Map<BlogReadDto>(blogModel);
			return CreatedAtRoute(nameof(GetBlogById), new { Id = blogRead.Id }, blogRead);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateBlog(int id, BlogUpdateDto updateDto)
		{
			var existingBlog = await _repository.GetBlogAsync(id);
			if (existingBlog is null)
			{
				return NotFound();
			}
			var blogModel = _mapper.Map<Blog>(updateDto);
			await _repository.UpdateBlog(blogModel);
			await _repository.SaveChanges();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteBlog(int id)
		{
			var existningBlog = await _repository.GetBlogAsync(id);
			if (existningBlog is null)
			{
				return NotFound();
			}
			await _repository.DeleteBlog(id);
			await _repository.SaveChanges();
			return NoContent();
		}
	}
}
