using AutoMapper;
using Blogvio.WebApi.Data;
using Blogvio.WebApi.Dtos.v1.Identity;
using Blogvio.WebApi.Models.Identity;
using Blogvio.WebApi.Models.SQLServerModels;
using Blogvio.WebApi.Seetings;
using Cosmonaut.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blogvio.WebApi.Infrastructure.Services
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly JWT _jwt;
		private readonly IMapper _mapper;
		private readonly TokenValidationParameters _tokenValidationParameters;
		private readonly AppDbContext _context;

		public IdentityService(
			UserManager<AppUser> userManager,
			IOptions<JWT> jwt,
			IMapper mapper,
			TokenValidationParameters tokenValidationParameters,
			AppDbContext context)
		{
			_userManager = userManager;
			_jwt = jwt.Value;
			_mapper = mapper;
			_tokenValidationParameters = tokenValidationParameters;
			_context = context;
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
			var jwtSecurityToken = await CreateJwtTokenAsync(user);
			var refreshToken = await CreateRefreshTokenAsync(jwtSecurityToken, user);
			return new AuthenticationModel
			{
				Email = user.Email,
				UserName = user.UserName,
				IsAuthenticated = true,
				Roles = new List<string> { "User" },
				Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
				ExpireOn = jwtSecurityToken.ValidTo,
				RefreshToken = refreshToken.Token
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

			var jwtSecurityToken = await CreateJwtTokenAsync(user);
			var rolesList = await _userManager.GetRolesAsync(user);
			var refreshToken = await CreateRefreshTokenAsync(jwtSecurityToken, user);

			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			authModel.Email = user.Email;
			authModel.UserName = user.UserName;
			authModel.ExpireOn = jwtSecurityToken.ValidTo;
			authModel.Roles = rolesList.ToList();

			return authModel;
		}

		private async Task<JwtSecurityToken> CreateJwtTokenAsync(AppUser user)
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
				expires: DateTime.Now.Add(_jwt.TokenLifeTime),
				signingCredentials: signingCredentials
			);
			return jwtSecurityToken;
		}

		public async Task<AuthenticationModel> RefreshTokenAsync(string token, string refreshToken)
		{
			var authModel = new AuthenticationModel();
			var validatedToken = GetPrincipalFromToken(token);
			if (validatedToken is null)
			{
				authModel.Message = "Invalid Token!";
				return authModel;
			}
			var expiryDateUnix = long.Parse(
				validatedToken.Claims.Single(x =>
					x.Type == JwtRegisteredClaimNames.Exp).Value);

			var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
				.AddSeconds(expiryDateUnix);

			if (expiryDateTimeUtc > DateTime.UtcNow)
			{
				authModel.Message = "Token hasn't expired yet!";
				return authModel;
			}

			var jti = validatedToken.Claims.Single(x =>
					x.Type == JwtRegisteredClaimNames.Jti).Value;

			var storedRefreshToken = await _context.RefreshTokens
				.SingleOrDefaultAsync(x => x.Token == refreshToken);
			if (storedRefreshToken is null)
			{
				authModel.Message = "Refresh token dosen't exist!";
				return authModel;
			}
			if (DateTime.UtcNow > storedRefreshToken.ExpiresOn)
			{
				authModel.Message = "Refresh token has expired!";
				return authModel;
			}
			if (storedRefreshToken.IsInvalidated)
			{
				authModel.Message = "Refresh token has been invalidated!";
				return authModel;
			}
			if (storedRefreshToken.IsUsed)
			{
				authModel.Message = "Refresh token has been used!";
				return authModel;
			}
			if (storedRefreshToken.JwtId != jti)
			{
				authModel.Message = "Refresh token dosen't match the jwt!";
				return authModel;
			}
			storedRefreshToken.IsUsed = true;
			_context.RefreshTokens.Update(storedRefreshToken);
			await _context.SaveChangesAsync();

			var user = await _userManager.FindByIdAsync(
				validatedToken.Claims.Single(x =>
					x.Type == "id").Value);

			var newToken = await CreateJwtTokenAsync(user);
			var newRefreshToken = await CreateRefreshTokenAsync(newToken, user);

			var rolesList = await _userManager.GetRolesAsync(user);
			authModel.IsAuthenticated = true;
			authModel.Token = new JwtSecurityTokenHandler().WriteToken(newToken);
			authModel.Email = user.Email;
			authModel.UserName = user.UserName;
			authModel.ExpireOn = newToken.ValidTo;
			authModel.Roles = rolesList.ToList();
			authModel.RefreshToken = newRefreshToken.Token;
			return authModel;
		}

		private async Task<RefreshToken> CreateRefreshTokenAsync(JwtSecurityToken jwtSecurityToken, AppUser user)
		{
			var refreshToken = new RefreshToken
			{
				JwtId = jwtSecurityToken.Id,
				UserId = user.Id,
				CreatedOn = DateTime.UtcNow,
				ExpiresOn = DateTime.UtcNow.AddMonths(6),
			};
			await _context.RefreshTokens.AddAsync(refreshToken);
			await _context.SaveChangesAsync();
			return refreshToken;
		}

		private ClaimsPrincipal GetPrincipalFromToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				var Principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
				if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
				{
					return null;
				}
				return Principal;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
			=> validatedToken is JwtSecurityToken jwtSecurityToken &&
					jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
						StringComparison.InvariantCultureIgnoreCase);
	}
}
