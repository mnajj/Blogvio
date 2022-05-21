using AutoMapper;
using Blogvio.WebApi.Controllers;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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

		[Fact]
		public async void GetPostAsync_WithEistingPost_ReturnsPost()
		{
			// Arrange
			var sendePost = CreateRandomPost();
			var expectedPostDto = MapperStub.Object.Map<PostReadDto>(sendePost);

			RepoStub.Setup(r => r.BlogExist(It.IsAny<int>()))
				.ReturnsAsync(true);

			RepoStub.Setup(r => r.GetPostAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(sendePost);

			var controller = new PostController(RepoStub.Object, MapperStub.Object);

			// Act
			var result = await controller.GetPostAsync(RandomNumber.Next(), RandomNumber.Next());

			// Assert
			result.Value.Should().BeEquivalentTo(expectedPostDto);
		}

		[Fact]
		public async void GetPostAsync_WithUnEistingPost_ReturnsNotFound()
		{
			// Arrange
			var sendePost = CreateRandomPost();
			var expectedPostDto = MapperStub.Object.Map<PostReadDto>(sendePost);

			RepoStub.Setup(r => r.BlogExist(It.IsAny<int>()))
				.ReturnsAsync(true);

			RepoStub.Setup(r => r.GetPostAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync((Post)null);

			var controller = new PostController(RepoStub.Object, MapperStub.Object);

			// Act
			var result = await controller.GetPostAsync(RandomNumber.Next(), RandomNumber.Next());

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async void GetPostAsync_WithUnEistingBlog_ReturnsNotFound()
		{
			// Arrange
			var sendePost = CreateRandomPost();
			var expectedPostDto = MapperStub.Object.Map<PostReadDto>(sendePost);

			RepoStub.Setup(r => r.BlogExist(It.IsAny<int>()))
				.ReturnsAsync(false);

			RepoStub.Setup(r => r.GetPostAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(sendePost);

			var controller = new PostController(RepoStub.Object, MapperStub.Object);

			// Act
			var result = await controller.GetPostAsync(RandomNumber.Next(), RandomNumber.Next());

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		private Post CreateRandomPost()
			=> new()
			{
				Id = RandomNumber.Next(),
				PublishedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Content = Guid.NewGuid().ToString(),
				IsDeleted = false
			};

		private IEnumerable<Post> CreateRandomPostsList() =>
			new List<Post>()
			{
				CreateRandomPost(),
				CreateRandomPost(),
				CreateRandomPost()
			};

		private PostReadDto CreateRandomPostReadDto()
			=> new()
			{
				Id = RandomNumber.Next(),
				PublishedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Content = Guid.NewGuid().ToString(),
			};
	}
}
