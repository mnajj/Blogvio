namespace Blogvio.WebApi.Infrastructure.Exceptions;

public class DbCommitFailException : Exception
{
	public DbCommitFailException() { }
	public DbCommitFailException(string message) : base(message) { }
	public DbCommitFailException(string message, Exception innerException)
		: base(message, innerException) { }
	public DbCommitFailException(string name, object key)
		: base($"Entity \"{name}\" ({key}) was not found.") { }
}
