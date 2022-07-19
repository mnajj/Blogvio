namespace Blogvio.WebApi.Models.Response;

public class PageResponse<T>
{
	public PageResponse() { }

	public PageResponse(IEnumerable<T> data)
	{
		Data = data;
	}

	public IEnumerable<T> Data { get; set; }
	public int? PageNumber { get; set; }
	public int? PageSize { get; set; }
	public string NextPage { get; set; }
	public string PreviousPage { get; set; }
}