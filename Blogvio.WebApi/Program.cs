using Blogvio.WebApi.Configuration;
using Blogvio.WebApi.Data;
using Blogvio.WebApi.Extensions;
using Blogvio.WebApi.Infrastructure.Filters;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region CustomServices

builder.Services.ConfigureAppSettingsFile(builder.Configuration);
builder.Services.ConfigureSerilog(builder.Host);
builder.RegisterServices();
builder.RegisterAllDbs();
builder.Services.AddJWT(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidation(config =>
	config.RegisterValidatorsFromAssemblyContaining<Program>());
builder.Services.ConfigureApiBehavior();
builder.Services.ConfigureMediator();

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