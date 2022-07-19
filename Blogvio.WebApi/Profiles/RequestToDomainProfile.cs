using AutoMapper;
using Blogvio.WebApi.Dtos.Queries;
using Blogvio.WebApi.Models;

namespace Blogvio.WebApi.Profiles;

public class RequestToDomainProfile : Profile
{
	public RequestToDomainProfile()
	{
		CreateMap<PaginationQuery, PaginationFilter>();
	}
}