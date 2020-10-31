using Microsoft.AspNetCore.Mvc;
using Moq;
using RTL.TVShows.Api.Controllers;
using RTL.TVShows.Domain;
using RTL.TVShows.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RTL.TVShows.Api.Tests.Controllers
{
    public class TVShowsControllerTests
    {
        private readonly Mock<ITVShowsService> tVShowServiceMock;

        public TVShowsControllerTests()
        {
            tVShowServiceMock = new Mock<ITVShowsService>();
        }

        private TVShowsController CreateSut()
        {
            return new TVShowsController(tVShowServiceMock.Object);
        }

        #region TestData
        public static IEnumerable<object[]> NoTvShowsTestData()
        {
            yield return new object[] { null };
            yield return new object[] { new List<TVShow>() };
        }
        #endregion

        #region Constructor
        [Fact]
        public void Constructor_Returns_ArgumentNullException_When_TVShowService_Is_Null()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TVShowsController(null));
        }
        #endregion

        #region Get
        [Theory]
        [MemberData(nameof(NoTvShowsTestData))]
        public async Task Get_Returns_NotFound_When_TvShows_Is_Null_Or_Empty(IEnumerable<TVShow> tVShows)
        {
            // Arrange
            tVShowServiceMock
                .Setup(mock => mock.GetTVShowsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(tVShows);

            var sut = CreateSut();

            // Act
            var actual = await sut.Get();

            // Assert
            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task Get_Returns_OK_When_TVShows_Is_Not_Null()
        {
            // Arrange
            var expectedTVShows = new List<TVShow>
            {
                new TVShow(),
                new TVShow()
            };

            tVShowServiceMock
                .Setup(mock => mock.GetTVShowsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedTVShows);

            var sut = CreateSut();

            // Act
            var actual = await sut.Get();

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(actual);
            var redirects = Assert.IsAssignableFrom<IEnumerable<TVShow>>(okObject.Value);
            Assert.Equal(expectedTVShows.Count(), redirects.Count());
        }
        #endregion
    }
}
