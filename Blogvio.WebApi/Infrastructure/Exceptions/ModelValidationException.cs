namespace Blogvio.WebApi.Infrastructure.Exceptions
{
	public class ModelValidationException : Exception
	{
		public ModelValidationException() { }
		public ModelValidationException(string message) : base(message) { }
		public ModelValidationException(string message, Exception innerException)
			: base(message, innerException) { }
		public ModelValidationException(string name, object key)
			: base($"Entity \"{name}\" ({key}) was not found.") { }
	}
}
