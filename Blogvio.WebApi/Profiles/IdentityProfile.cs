using AutoMapper;
using Blogvio.WebApi.Dtos.v1.Identity;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.SQLServerModels;

namespace Blogvio.WebApi.Profiles
{
	public class IdentityProfile : Profile
	{
		public IdentityProfile()
		{
			CreateMap<RegisterDto, AppUser>()
				.ForSourceMember(s => s.Password, p => p.DoNotValidate());
		}
	}
}
