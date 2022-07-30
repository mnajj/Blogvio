using AutoMapper;
using Blogvio.WebApi.Controllers;
using Blogvio.WebApi.Dtos.Post;
using Blogvio.WebApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Blogvio.WebApi.Controllers.V1;
using Xunit;
using Blogvio.WebApi.Interfaces;

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
			result.Value.Should().BeEquivalentTo(
				result.Value as List<PostReadDto>,
				opts => opts.ComparingByMembers<Post>().ExcludingMissingMembers());
		}

		[Fact]
		public async void GetPostAsync_WithEistingPost_ReturnsPost()
		{
			// Arrange
			var post = CreateRandomPost();

			RepoStub.Setup(r => r.BlogExist(It.IsAny<int>()))
				.ReturnsAsync(true);

			RepoStub.Setup(r => r.GetPostAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(post);

			var controller = new PostController(RepoStub.Object, MapperStub.Object);

			// Act
			var result = await controller.GetPostAsync(RandomNumber.Next(), RandomNumber.Next());

			// Assert
			result.Value.Should().BeEquivalentTo(
				result.Value,
				opts => opts.ComparingByMembers<Post>().ExcludingMissingMembers());
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

		[Fact]
		public async void CreatePostAsync_WithPostToCreate_ReturnsPostReadDto()
		{
			// Arrange
			var postToCreate = CreateRandomCreatePost();
			var expectedPostDto = MapperStub.Object.Map<PostReadDto>(postToCreate);

			RepoStub.Setup(r => r.BlogExist(It.IsAny<int>()))
				.ReturnsAsync(true);

			RepoStub.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<PostCreateDto, Post>();
				cfg.CreateMap<Post, PostReadDto>();
			});
			var mapper = config.CreateMapper();

			var controller = new PostController(RepoStub.Object, mapper);

			// Act
			var result = await controller.CreatePostAsync(RandomNumber.Next(), postToCreate);

			// Assert
			result.Value.Should().BeEquivalentTo(
				result.Value,
				opts => opts.ComparingByMembers<PostCreateDto>().ExcludingMissingMembers());
		}

		[Fact]
		public async void UpdatePostAsync_WithPostToUpdate_ReturnsNoContent()
		{
			// Arrange
			var post = CreateRandomPost();
			var postToUpdate = CreateRandomUpdatePostDto();
			var expectedPostDto = MapperStub.Object.Map<PostReadDto>(postToUpdate);

			RepoStub.Setup(r => r.BlogExist(It.IsAny<int>()))
				.ReturnsAsync(true);
			RepoStub.Setup(r => r.GetPostAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(post);
			RepoStub.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);

			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<PostUpdateDto, Post>();
			});
			var mapper = config.CreateMapper();

			var controller = new PostController(RepoStub.Object, mapper);

			// Act
			var result = await controller.UpdatePostAsync(RandomNumber.Next(), RandomNumber.Next(), postToUpdate);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}

		[Fact]
		public async void DeletePostAsync_WithPostToDelete_ReturnsNoContent()
		{
			// Arrange
			var post = CreateRandomPost();

			RepoStub.Setup(r => r.BlogExist(It.IsAny<int>()))
				.ReturnsAsync(true);
			RepoStub.Setup(r => r.GetPostAsync(It.IsAny<int>(), It.IsAny<int>()))
				.ReturnsAsync(post);
			RepoStub.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
			var controller = new PostController(RepoStub.Object, MapperStub.Object);

			// Act
			var result = await controller.DeltePostAsync(post.BlogId, post.Id);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}

		private Post CreateRandomPost()
			=> new()
			{
				Id = RandomNumber.Next(),
				BlogId = RandomNumber.Next(),
				PublishedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Content = Guid.NewGuid().ToString(),
				IsDeleted = false
			};

		private PostCreateDto CreateRandomCreatePost()
			=> new()
			{
				Content = Guid.NewGuid().ToString()
			};

		private PostUpdateDto CreateRandomUpdatePostDto()
			=> new()
			{
				Content = Guid.NewGuid().ToString(),
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
