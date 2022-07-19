using System.Text;
using Blogvio.WebApi.Data;
using Blogvio.WebApi.Models.SQLServerModels;
using Blogvio.WebApi.Security;
using Blogvio.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Blogvio.WebApi.Extensions
{
	public static class Authentication
	{
		public static void AddJWT(
			this IServiceCollection services,
			IConfiguration configuration)
		{
			services.Configure<JWT>(configuration.GetSection("JWT"));
			services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidIssuer = configuration["JWT:Issuer"],
				ValidAudience = configuration["JWT:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
			};
			services.AddSingleton(tokenValidationParameters);
			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = false;
					options.TokenValidationParameters = tokenValidationParameters;
				});
			services.AddScoped<IIdentityService, IdentityService>();
		}
	}
}