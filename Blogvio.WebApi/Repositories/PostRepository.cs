using AutoMapper;
using Blogvio.WebApi.Data;
using Blogvio.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Blogvio.WebApi.Repositories
{
	public class PostRepository : IPostRepository
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public PostRepository(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		public async Task CreatePostAsync(Post post)
		{
			await _context.Posts.AddAsync(post);
		}

		public async Task DeletePostAsync(int id)
		{
			var post = await GetPostByIdAsync(id);
			post.IsDeleted = true;
		}

		public async Task<Post> GetPostByIdAsync(int id)
		{
			return await _context
				.Posts
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<IEnumerable<Post>> GetPostsAsync()
		{
			return await _context.Posts.ToListAsync();
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public async Task UpdatePostAsync(Post post)
		{
			_context.Entry(
				await _context.Posts
					.FirstOrDefaultAsync(p => p.Id == post.Id)
				)
				.CurrentValues
				.SetValues(post);
		}
	}
}
