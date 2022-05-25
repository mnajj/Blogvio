using Blogvio.WebApi.Dtos.v1.Identity;
using Blogvio.WebApi.Models.Identity;
using Blogvio.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blogvio.WebApi.Controllers.V1
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IIdentityService _identityService;

		public AuthController(IIdentityService identityService)
		{
			_identityService = identityService;
		}

		[HttpPost("register")]
		public async Task<ActionResult<AuthenticationModel>> RegisterAsync(RegisterDto registerDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var result = await _identityService.RegisterAsync(registerDto);
			if (!result.IsAuthenticated)
				return BadRequest(result.Message);
			return Ok(result);
		}

		[HttpPost("login")]
		public async Task<ActionResult<AuthenticationModel>> LoginAsync(LoginDto loginDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var result = await _identityService.LoginAsync(loginDto);
			if (!result.IsAuthenticated)
			{
				return BadRequest(result.Message);
			}
			return Ok(result);
		}
	}
}
