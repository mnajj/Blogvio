using AutoMapper;
using Blogvio.WebApi.Data;
using Blogvio.WebApi.Interfaces;
using Blogvio.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace Blogvio.WebApi.Repositories.Repository.SQLServer
{
	public class PostRepository : IPostRepository
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IElasticClient _elasticClient;

		public PostRepository(AppDbContext context, IMapper mapper, IElasticClient elasticClient)
		{
			_context = context;
			_mapper = mapper;
			_elasticClient = elasticClient;
		}

		public async Task CreatePostAsync(int blogId, Post post)
		{
			if (post is null)
			{
				throw new ArgumentNullException(nameof(post));
			}
			post.BlogId = blogId;
			await _context.Posts.AddAsync(post);
		}

		public async Task DeletePostAsync(int id)
		{
			var post = await _context.Posts
				.FirstOrDefaultAsync(p =>
					p.Id == id &&
					p.IsDeleted == false);
			post.IsDeleted = true;
		}

		public async Task<Post?> GetPostAsync(int blogId, int postId)
		{
			return await _context
				.Posts
				.FirstOrDefaultAsync(p =>
					p.BlogId == blogId &&
					p.Id == postId &&
					!p.IsDeleted);
		}

		public async Task<IEnumerable<Post>> GetPostsForBlogAsync(int blogId, PaginationFilter? paginationFilter)
		{
			if (paginationFilter is null)
			{
				return await _context.Posts
					.Where(p => p.BlogId == blogId
					&& !p.IsDeleted)
					.ToListAsync();
			}
			var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
			return await _context.Posts
				.Where(b => !b.IsDeleted)
				.Skip(skip)
				.Take(paginationFilter.PageSize)
				.ToListAsync();
		}

		public async Task<bool> SaveChangesAsync()
		{
			return (await _context.SaveChangesAsync() >= 0);
		}

		public async Task UpdatePostAsync(int blogId, Post post)
		{
			var exPost = await _context.Posts
					.FirstOrDefaultAsync(p =>
						p.Id == post.Id &&
						p.BlogId == blogId &&
						p.IsDeleted == false);
			exPost.Content = post.Content;
			exPost.UpdatedAt = DateTime.Now;
		}

		public async Task<bool> IsBlogExist(int blogId)
		{
			return await _context
				.Blogs
				.AnyAsync(b => b.Id == blogId &&
					!b.IsDeleted);
		}

		public async Task<IEnumerable<Post>> SearchForPostAsync(string keyword)
		{
			return (await _elasticClient
				.SearchAsync<Post>(s =>
					s.Query(q =>
						q.QueryString(d =>
							d.Query('*' + keyword + '*')))
							.Size(1000)
							)).Documents.ToList();
		}
	}
}
