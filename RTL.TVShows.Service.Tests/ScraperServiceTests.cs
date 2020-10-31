using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RTL.TVShows.Infrastructure.Interfaces;
using RTL.TVShows.Infrastructure.Settings;
using System;
using Xunit;

namespace RTL.TVShows.Service.Tests
{
	public class ScraperServiceTests
	{
		private readonly Mock<ILogger<ScraperService>> loggerMock;
		private readonly Mock<ITVShowsRepository> tvShowRepositoryMock;
		private readonly Mock<ITVMazeHttpClient> tvMazeHttpClientMock;
		private readonly Mock<IOptions<ScraperSettings>> optionsMock;

		public ScraperServiceTests()
		{
			loggerMock = new Mock<ILogger<ScraperService>>();
			tvShowRepositoryMock = new Mock<ITVShowsRepository>();
			tvMazeHttpClientMock = new Mock<ITVMazeHttpClient>();
			optionsMock = new Mock<IOptions<ScraperSettings>>();
		}

		private ScraperService CreateSut()
		{
			return new ScraperService(loggerMock.Object, tvShowRepositoryMock.Object, tvMazeHttpClientMock.Object, optionsMock.Object);
		}

		#region Constructor
		[Fact]
		public void Constructor_Throws_ArgumentNullException_When_Logger_Is_Null()
		{
			// Arrange, Act & Assert
			Assert.Throws<ArgumentNullException>(() => new ScraperService(null, tvShowRepositoryMock.Object,
				tvMazeHttpClientMock.Object, optionsMock.Object));
		}

		[Fact]
		public void Constructor_Throws_ArgumentNullException_When_TVShowsRepository_Is_Null()
		{
			// Arrange, Act & Assert
			Assert.Throws<ArgumentNullException>(() => new ScraperService(loggerMock.Object, null,
				tvMazeHttpClientMock.Object, optionsMock.Object));
		}

		[Fact]
		public void Constructor_Throws_ArgumentNullException_When_HttpClient_Is_Null()
		{
			// Arrange, Act & Assert
			Assert.Throws<ArgumentNullException>(() => new ScraperService(loggerMock.Object, tvShowRepositoryMock.Object,
				null, optionsMock.Object));
		}

		[Fact]
		public void Constructor_Throws_ArgumentNullException_When_Settings_Are_Null()
		{
			// Arrange, Act & Assert
			Assert.Throws<ArgumentNullException>(() => new ScraperService(loggerMock.Object, tvShowRepositoryMock.Object,
				tvMazeHttpClientMock.Object, null));
		}
		#endregion
	}
}
