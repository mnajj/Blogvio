using Blogvio.WebApi.Data;
using Blogvio.WebApi.Extensions;
using Blogvio.WebApi.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

#region CustomServices

ConfigureAppSettingsFile();
ConfigureLogs();
builder.Host.UseSerilog();
builder.RegisterServices();
builder.RegisterAllDbs();
// builder.Services.InstallCosmosDb(builder.Configuration);
builder.Services.AddJWT(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidation(config =>
	config.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

#endregion

builder.Services.AddControllers(options =>
{
	options.Filters.Add<ValidationFilter>();
	options.Filters.Add(new ApiExceptionFilterAttribute());
});
builder.AddAPIDocumentation();

var app = builder.Build();

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