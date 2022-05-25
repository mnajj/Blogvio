using AutoMapper;
using Blogvio.WebApi.Dtos.v1.Identity;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Models.Identity;
using Blogvio.WebApi.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blogvio.WebApi.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly JWT _jwt;
		private readonly IMapper _mapper;

		public IdentityService(UserManager<AppUser> userManager, IOptions<JWT> jwt, IMapper mapper)
		{
			_userManager = userManager;
			_jwt = jwt.Value;
			_mapper = mapper;
		}

		public async Task<AuthenticationModel> RegisterAsync(RegisterDto model)
		{
			if (await _userManager.FindByEmailAsync(model.Email) is not null)
			{
				return new AuthenticationModel
				{
					Message = "Email is already registerd!",
				};
			}
			if (await _userManager.FindByNameAsync(model.UserName) is not null)
			{
				return new AuthenticationModel
				{
					Message = "User Name is already registerd!",
				};
			}
			var user = _mapper.Map<AppUser>(model);
			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				var errors = string.Empty;
				foreach (var err in result.Errors)
				{
					errors += $"{err.Description}, ";
				}
				return new AuthenticationModel { Message = errors };
			}
			await _userManager.AddToRoleAsync(user, "User");
			var jwtSecurityToken = await CreateJwtToken(user);
			return new AuthenticationModel
			{
				Email = user.Email,
				UserName = user.UserName,
				IsAuthenticated = true,
				Roles = new List<string> { "User" },
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				ExpireOn = jwtSecurityToken.ValidTo
			};
		}

		public async Task<AuthenticationModel> LoginAsync(LoginDto model)
		{
			var authModel = new AuthenticationModel();

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				authModel.Message = "Email or Password is incorrect!";
				return authModel;
			}

			var jwtSecurityToken = await CreateJwtToken(user);
			var rolesList = await _userManager.GetRolesAsync(user);

			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			authModel.Email = user.Email;
			authModel.UserName = user.UserName;
			authModel.ExpireOn = jwtSecurityToken.ValidTo;
			authModel.Roles = rolesList.ToList();

			return authModel;
		}

		private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim("UserId", user.Id),
				new Claim("UserName", user.UserName),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddHours(_jwt.DurationInDays),
				signingCredentials: signingCredentials
			);
			return jwtSecurityToken;
		}
	}
}
