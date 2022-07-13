using Blogvio.WebApi.Data;
using Blogvio.WebApi.Extenstions;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using Blogvio.WebApi.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

#region CustomServices
ConfigureAppSettingsFile();
ConfigureLogs();
builder.Host.UseSerilog();
builder.Services.AddJWT(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddElasticSearch(builder.Configuration);
builder.Services.InstallCosmosDb(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

PrebDb.PrepSqlServerDatabase(app);

app.Run();

#region Helper
void ConfigureLogs()
{
	Log.Logger = new LoggerConfiguration()
		.Enrich.FromLogContext()
		.Enrich.WithExceptionDetails()
		.WriteTo.Debug()
		.WriteTo.Console()
		.CreateLogger();
}

void ConfigureAppSettingsFile()
{
	var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
	builder.Configuration
	.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
	.AddJsonFile($"appsettings.json")
	.AddJsonFile($"appsettings.{env}.json");
}
#endregion