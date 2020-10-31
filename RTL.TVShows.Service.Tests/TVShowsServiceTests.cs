using Moq;
using RTL.TVShows.Domain;
using RTL.TVShows.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RTL.TVShows.Service.Tests
{
    public class TVShowsServiceTests
    {
        private readonly Mock<ITVShowsRepository> tvShowRepositoryMock;

        public TVShowsServiceTests()
        {
            tvShowRepositoryMock = new Mock<ITVShowsRepository>();
        }

        private TVShowsService CreateSut()
        {
            return new TVShowsService(tvShowRepositoryMock.Object);
        }

        #region Constructor
        [Fact]
        public void Constructor_Throws_ArgumentNullException_When_TVShowsRepository_Is_Null()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TVShowsService(null));
        }
        #endregion

        #region GetTVShowsAsync
        [Theory]
        [InlineData(1,0)]
        [InlineData(1,-1)]
        [InlineData(0,1)]
        [InlineData(-1,1)]
        public async Task GetTVShowsAsync_Returns_Null_When_PageNumber_Or_PageSize_Is_Less_Then_1(int pageNumber, int pageSize)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var actual = await sut.GetTVShowsAsync(pageNumber, pageSize);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetTVShowsAsync_Returns_TVShows_When_PageNumber_And_PageSize_Are_Correct()
        {
            // Arrange
            var expectedTVShows = new List<TVShow>() {
                new TVShow(){ Id = 1, Name = "TVShow1", Cast = new List<Cast>{ new Cast { } } },
                new TVShow(){ Id = 2, Name = "TVShow2", Cast = new List<Cast>{ new Cast { } } }
            };

            tvShowRepositoryMock
                .Setup(mock => mock.GetTVShowsAsync())
                .ReturnsAsync(expectedTVShows);

            var sut = CreateSut();

            // Act
            var actual = await sut.GetTVShowsAsync(1,10);

            // Assert
            Assert.Equal(2, actual.Count());
        }
        #endregion
    }
}
