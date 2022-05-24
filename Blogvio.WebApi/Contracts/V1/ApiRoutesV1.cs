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

		public static class Post
		{
			public const string PostBase = Base + "/post/{blogId}";
			public const string GetPostAsync = "{postId}";
			public const string UpdatePostAsync = "{postId}";
			public const string DeletePostAsync = "{postId}";
			public const string SearchForPostAsync = "search/{keyword}";
		}
	}
}
