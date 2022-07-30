using Blogvio.WebApi.Dtos.v1.Identity;
using Blogvio.WebApi.Infrastructure.Services;
using Blogvio.WebApi.Models.Identity;
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
			var result = await _identityService.RegisterAsync(registerDto);
			if (!result.IsAuthenticated)
				return BadRequest(result.Message);
			return Ok(result);
		}

		[HttpPost("login")]
		public async Task<ActionResult<AuthenticationModel>> LoginAsync(LoginDto loginDto)
		{
			var result = await _identityService.LoginAsync(loginDto);
			if (!result.IsAuthenticated)
			{
				return BadRequest(result.Message);
			}
			return Ok(result);
		}

		[HttpPost("refresh")]
		public async Task<ActionResult<AuthenticationModel>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
		{
			var result = await _identityService.RefreshTokenAsync(refreshTokenDto.Token, refreshTokenDto.RefreshToken);
			if (!result.IsAuthenticated)
			{
				return BadRequest(result.Message);
			}
			return Ok(result);
		}
	}
}
