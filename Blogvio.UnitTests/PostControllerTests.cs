using AutoMapper;
using Blogvio.WebApi.Controllers;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Blogvio.UnitTests
{
	public class PostControllerTests
	{
		public Mock<IMapper> MapperStub { get; set; } = new Mock<IMapper>();
		public Mock<IPostRepository> RepoStub { get; set; } = new Mock<IPostRepository>();
		public Random RandomNumber { get; set; } = new();

		[Fact]
		public async void GetPostsAsync_WithEistingPosts_ReturnsAllPosts()
		{
			// Arrange
			var expectedPosts = CreateRandomPostsList();
			RepoStub.Setup(r => r.GetPostsForBlogAsync(It.IsAny<int>()))
				.ReturnsAsync(expectedPosts);
			var controller = new PostController(RepoStub.Object, MapperStub.Object);

			// Act
			var result = await controller.GetPostsForBlogAsync(It.IsAny<int>());

			// Assert
			var postDto = result.Value as List<PostReadDto>;
			result.Value.Should().BeEquivalentTo(postDto);
		}

		private IEnumerable<Post> CreateRandomPostsList() =>
			new List<Post>()
			{
				new Post()
				{
					Id = RandomNumber.Next(),
					PublishedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					Content = Guid.NewGuid().ToString(),
					IsDeleted = false
				},
				new Post()
				{
					Id = RandomNumber.Next(),
					PublishedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					Content = Guid.NewGuid().ToString(),
					IsDeleted = false
				},
				new Post()
				{
					Id = RandomNumber.Next(),
					PublishedAt = DateTime.Now,
					UpdatedAt = DateTime.Now,
					Content = Guid.NewGuid().ToString(),
					IsDeleted = false
				}
			};
	}
}
