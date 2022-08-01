using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Models.Response;
using MediatR;

namespace Blogvio.WebApi.Queries.PostQueries;
public record GetPostForBlogQuery(int BlogId, int PostId)
	: IRequest<Response<PostReadDto>>;
