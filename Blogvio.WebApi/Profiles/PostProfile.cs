using AutoMapper;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Models;

namespace Blogvio.WebApi.Profiles
{
	public class PostProfile : Profile
	{
		public PostProfile()
		{
			CreateMap<Post, PostReadDto>();
			CreateMap<PostCreateDto, Post>();
			CreateMap<PostCreateDto, PostReadDto>();
			CreateMap<PostUpdateDto, Post>();
		}
	}
}
