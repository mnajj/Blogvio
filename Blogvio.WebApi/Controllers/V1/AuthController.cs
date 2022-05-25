using Blogvio.WebApi.Dtos.v1.Identity;
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
		public async Task<IActionResult> RegisterAsync(RegisterDto model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _identityService.RegisterAsync(model);
			if (!result.IsAuthenticated)
				return BadRequest(result.Message);
			return Ok(result);
		}
	}
}
