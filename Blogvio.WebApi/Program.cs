using Blogvio.WebApi.Data;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
		.WriteTo.Console()
		.CreateBootstrapLogger();

Log.Information("Starting up");

//try
//{
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//
builder.Configuration
	.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
	.AddJsonFile($"appsettings.json")
	.AddJsonFile($"appsettings.{builder.Configuration.GetSection("Environments").Value}.json");

//var config = new ConfigurationBuilder()
//	.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//	.AddJsonFile($"appsettings.json")
//	.AddJsonFile($"appsettings.{builder.Configuration.GetSection("Environments").Value}.json")
//	.Build();

//Log.Logger = new LoggerConfiguration()
//	.ReadFrom.Configuration(config)
//	.CreateLogger();

builder.Host.UseSerilog((ctx, lc) => lc
		.WriteTo.Console()
		.ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrebDb.PrepSqlServerDatabase(app);

app.Run();
//}
//catch (Exception ex)
//{
//	Log.Fatal(ex, "Application failed to start!");
//}
//finally
//{
//	Log.Information("Shut down...");
//	Log.CloseAndFlush();
//}