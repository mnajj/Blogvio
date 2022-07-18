using AutoMapper;
using Blogvio.WebApi.Contracts.V1;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers.V1
{
	[Route(ApiRoutesV1.Blogs.BlogBase)]
	[ApiController]
	public class BlogController : ControllerBase
	{
		private readonly IBlogRepository _repository;
		private readonly IMapper _mapper;

		public BlogController(
			IBlogRepository repository,
			IMapper mapper
		)
		{
			_repository = repository;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BlogReadDto>>> GetBlogsAsync()
		{
			var blogs = await _repository.GetBlogsAsync();
			return Ok(_mapper.Map<IEnumerable<BlogReadDto>>(blogs));
		}

		[HttpGet(ApiRoutesV1.Blogs.GetBlogByIdAsync, Name = "GetBlogByIdAsync")]
		public async Task<ActionResult<BlogReadDto>> GetBlogByIdAsync(int id)
		{
			var blog = await _repository.GetBlogAsync(id);
			if (blog is null)
			{
				return NotFound();
			}
			var blogReadDto = _mapper.Map<BlogReadDto>(blog);
			return Ok(blogReadDto);
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
			var blogRead = _mapper.Map<BlogReadDto>(blogModel);
			return CreatedAtRoute(nameof(GetBlogByIdAsync), new { Id = blogRead.Id }, blogRead);
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
}
