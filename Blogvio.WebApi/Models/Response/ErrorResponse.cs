namespace Blogvio.WebApi.Models.Response;

public class ErrorResponse
{
	public List<ErrorModel> Errors { get; set; } = new();
}