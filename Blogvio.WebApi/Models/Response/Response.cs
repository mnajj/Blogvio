namespace Blogvio.WebApi.Models.Response;

public class Response<T>
{
	public T Data { get; set; }
	public string? Message { get; set; }

	public Response() { }

	public Response(T data)
	{
		Data = data;
	}
}