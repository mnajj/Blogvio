namespace Blogvio.WebApi.Dtos.Queries;

public class PaginationQuery
{
	public PaginationQuery()
	{
		PageNumber = 1;
		PageSize = 10;
	}

	public PaginationQuery(int pageNumber, int sizePage)
	{
		PageNumber = pageNumber;
		PageSize = sizePage > 10 ? 10 : sizePage;
	}

	public int PageNumber { get; set; }
	public int PageSize { get; set; }
}