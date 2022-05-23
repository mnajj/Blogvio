namespace Blogvio.WebApi.Contracts.V1
{
	public static class ApiRoutesV1
	{
		public const string Root = "api";
		public const string Version = "v1";
		public const string Base = $"{Root}/{Version}";

		public static class Blogs
		{
			public const string BlogBase = $"{Base}/blog";
			public const string GetBlogsAsync = $"{Base}/blog";
			public const string GetBlogByIdAsync = "{id}";
			public const string UpdateBlogAsync = "{id}";
			public const string DeleteBlogAsync = "{id}";
		}
	}
}
