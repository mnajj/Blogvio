using AutoMapper;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Models;

namespace Blogvio.WebApi.Profiles
{
	public class BlogProfile : Profile
	{
		public BlogProfile()
		{
			CreateMap<Blog, BlogReadDto>();
			CreateMap<BlogCreateDto, Blog>();
			CreateMap<BlogCreateDto, BlogReadDto>();
			CreateMap<BlogUpdateDto, Blog>();
		}
	}
}
