using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using RTL.TVShows.Domain;
using RTL.TVShows.Extensions.Interfaces;
using RTL.TVShows.Infrastructure.Models;
using RTL.TVShows.Infrastructure.Repositories;
using RTL.TVShows.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RTL.TVShows.Infrastructure.Tests.Repositories
{
    public class TVShowsRepositoryTests
    {
        private readonly Mock<IOptions<TVShowsDatabaseSettings>> optionsMock;
        private readonly Mock<IMapper<TVShowModel, TVShow>> tvShowMapperMock;
        private readonly Mock<IMongoCollection<TVShowModel>> tvShowsCollectionMock;
        private readonly Mock<IMongoDatabase> mongoDatabaseMock;
        private readonly Mock<IMongoClient> mongoClientMock;

        public TVShowsRepositoryTests()
        {
            optionsMock = new Mock<IOptions<TVShowsDatabaseSettings>>();
            tvShowMapperMock = new Mock<IMapper<TVShowModel, TVShow>>();
            tvShowsCollectionMock = new Mock<IMongoCollection<TVShowModel>>();
            mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoClientMock = new Mock<IMongoClient>();

            optionsMock
                .Setup(mock => mock.Value)
                .Returns(new TVShowsDatabaseSettings
                {
                    ConnectionString = "mongodb://123",
                    DatabaseName = "DatabaseName",
                    TVShowsCollectionName = "TVShowsCollectionName"
                });

            mongoDatabaseMock
                .Setup(mock => mock.GetCollection<TVShowModel>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(tvShowsCollectionMock.Object);

            mongoClientMock
                .Setup(mock => mock.GetDatabase(optionsMock.Object.Value.DatabaseName, null))
                .Returns(mongoDatabaseMock.Object);
        }

        private TVShowsRepository CreateSut()
        {
            return new TVShowsRepository(optionsMock.Object, tvShowMapperMock.Object);
        }

        #region Constructor
        [Fact]
        public void Constructor_Throws_ArgumentNullException_When_Settings_Are_Null()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TVShowsRepository(null, tvShowMapperMock.Object));
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_When_TVShowMapper_Is_Null()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new TVShowsRepository(optionsMock.Object, null));
        }
        #endregion
    }
}
