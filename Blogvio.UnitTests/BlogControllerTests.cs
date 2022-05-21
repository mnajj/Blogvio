using AutoMapper;
using Blogvio.WebApi.Controllers;
using Blogvio.WebApi.Dtos.Blog;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Blogvio.UnitTests
{
	public class BlogControllerTests
	{
		public Mock<IMapper> MapperStub { get; set; } = new Mock<IMapper>();
		public Mock<IBlogRepository> RepoStub { get; set; } = new Mock<IBlogRepository>();
		public Random RandomNumber { get; set; } = new();

		[Fact]
		public async void GetBlogs_WithEistingBlogs_ReturnsAllBlogs()
		{
			var expectedBlogs = CreateRandomBlogsList();
			// Arrange
			RepoStub.Setup(repo => repo.GetBlogsAsync())
				.ReturnsAsync(expectedBlogs);

			var ctr = new BlogController(RepoStub.Object, MapperStub.Object);

			// Act
			var res = await ctr.GetBlogsAsync();

			// Assert
			var blogsDto = res.Value as List<BlogReadDto>;
			res.Value.Should().BeEquivalentTo(blogsDto);
		}

		[Fact]
		public async void GetBlogAsync_WithUnexistingItem_ReturnsNotFound()
		{
			// Arrange
			RepoStub.Setup(repo => repo.GetBlogAsync(It.IsAny<int>()))
				.ReturnsAsync((Blog)null);

			var ctr = new BlogController(RepoStub.Object, MapperStub.Object);

			// Act
			var res = await ctr.GetBlogByIdAsync(new Random().Next());

			// Assert
			Assert.IsType<NotFoundResult>(res.Result);
		}

		[Fact]
		public async void GetBlogAsync_WithExistingItem_ReturnsExpectedBlog()
		{
			// Arrange
			var expectedBlog = CreateRandomBlog();

			RepoStub.Setup(repo => repo.GetBlogAsync(It.IsAny<int>()))
				.ReturnsAsync(expectedBlog);

			var ctr = new BlogController(RepoStub.Object, MapperStub.Object);

			// Act
			var result = await ctr.GetBlogByIdAsync(new Random().Next());

			// Assert
			var blogDto = result.Value as BlogReadDto;
			result.Value.Should().BeEquivalentTo(blogDto);
		}

		[Fact]
		public async void CreateBlogAsync_WithBlogToCreate_ReturnsCreatedBlog()
		{
			// Arrange
			BlogCreateDto blogToCreate = new()
			{
				Url = Guid.NewGuid().ToString(),
			};
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<BlogCreateDto, Blog>();
				cfg.CreateMap<Blog, BlogReadDto>();
				cfg.CreateMap<BlogReadDto, Blog>();
			});
			var mapper = config.CreateMapper();

			var ctr = new BlogController(RepoStub.Object, mapper);

			// Act
			var result = await ctr.CreateBlogAsync(blogToCreate);

			// Assert
			var createdBlog = result.Value;
			result.Value.Should().BeEquivalentTo(
				createdBlog,
				opts => opts.ComparingByMembers<BlogCreateDto>().ExcludingMissingMembers()
				);
		}

		[Fact]
		public async void UpdateBlogAsync_WithExistingBlog_ReturnsNoContent()
		{
			// Arrange
			var existingBlog = CreateRandomBlog();
			var blogToUpdate = CreateRandomBlogUpdateDto();

			RepoStub.Setup(r => r.GetBlogAsync(It.IsAny<int>()))
				.ReturnsAsync(existingBlog);
			RepoStub.Setup(r => r.SaveChangesAsync())
				.ReturnsAsync(true);
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<BlogUpdateDto, Blog>();
			});
			var mapper = config.CreateMapper();

			var ctr = new BlogController(RepoStub.Object, mapper);

			// Act
			var result = await ctr.UpdateBlogAsync(existingBlog.Id, blogToUpdate);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}

		[Fact]
		public async void UpdateBlogAsync_WithUnexistingBlog_ReturnsNoFound()
		{
			// Arrange
			var existingBlog = CreateRandomBlog();
			var blogToUpdate = CreateRandomBlogUpdateDto();

			RepoStub.Setup(r => r.GetBlogAsync(RandomNumber.Next()))
				.ReturnsAsync(existingBlog);
			var config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<BlogUpdateDto, Blog>();
			});
			var mapper = config.CreateMapper();

			var ctr = new BlogController(RepoStub.Object, mapper);

			// Act
			var result = await ctr.UpdateBlogAsync(existingBlog.Id, blogToUpdate);

			// Assert
			result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async void DeleteBlogAsync_WithExistingBlog_ReturnsNoContent()
		{
			// Arrange
			var existingBlog = CreateRandomBlog();

			RepoStub.Setup(r => r.GetBlogAsync(It.IsAny<int>()))
				.ReturnsAsync(existingBlog);
			RepoStub.Setup(r => r.SaveChangesAsync())
				.ReturnsAsync(true);

			var ctr = new BlogController(RepoStub.Object, MapperStub.Object);

			// Act
			var result = await ctr.DeleteBlogAsync(existingBlog.Id);

			// Assert
			result.Should().BeOfType<NoContentResult>();
		}

		private Blog CreateRandomBlog() =>
			new()
			{
				Id = new Random().Next(),
				CreatedAt = DateTime.Now,
				Url = Guid.NewGuid().ToString(),
				IsDeleted = false
			};

		private List<Blog> CreateRandomBlogsList() =>
			new()
			{
				new Blog()
				{
					Id = new Random().Next(),
					CreatedAt = DateTime.Now,
					Url = Guid.NewGuid().ToString(),
				},
				new Blog()
				{
					Id = new Random().Next(),
					CreatedAt = DateTime.Now,
					Url = Guid.NewGuid().ToString(),
				},
				new Blog()
				{
					Id = new Random().Next(),
					CreatedAt = DateTime.Now,
					Url = Guid.NewGuid().ToString(),
				}
			};

		private BlogUpdateDto CreateRandomBlogUpdateDto() =>
			new()
			{
				Url = Guid.NewGuid().ToString(),
			};
	}
}
