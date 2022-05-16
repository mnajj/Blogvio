using AutoMapper;
using Blogvio.WebApi.Controllers;
using Blogvio.WebApi.Models;
using Blogvio.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace Blogvio.UnitTests
{
	public class BlogControllerTests
	{
		[Fact]
		public async void GetBlogAsync_WithUnexistingItem_ReturnsNotFound()
		{
			// Arrange
			var mapperStub = new Mock<IMapper>();
			var repoStub = new Mock<IBlogRepository>();
			repoStub.Setup(repo => repo.GetBlogAsync(new Random().Next()))
				.ReturnsAsync((Blog)null);

			var ctr = new BlogController(repoStub.Object, mapperStub.Object);

			// Act
			var res = await ctr.GetBlogById(new Random().Next());

			// Assert
			Assert.IsType<NotFoundResult>(res.Result);
		}
	}
}
