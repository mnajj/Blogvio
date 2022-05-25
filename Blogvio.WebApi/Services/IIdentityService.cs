using Blogvio.WebApi.Dtos.v1.Identity;
using Blogvio.WebApi.Models.Identity;

namespace Blogvio.WebApi.Services
{
	public interface IIdentityService
	{
		Task<AuthenticationModel> RegisterAsync(RegisterDto model);
		Task<AuthenticationModel> LoginAsync(LoginDto model);
	}
}
