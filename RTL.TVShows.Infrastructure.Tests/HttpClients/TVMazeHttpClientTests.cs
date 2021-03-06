﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using RTL.TVShows.Infrastructure.HttpClients;
using RTL.TVShows.Infrastructure.Settings;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RTL.TVShows.Infrastructure.Tests.HttpClients
{
	public class TVMazeHttpClientTests
	{
		private readonly Mock<HttpMessageHandler> httpMessageHandlerMock;
		private readonly Mock<ILogger<TVMazeHttpClient>> loggerMock;
		private readonly Mock<IOptions<ScraperSettings>> optionsMock;

		public TVMazeHttpClientTests()
		{
			httpMessageHandlerMock = new Mock<HttpMessageHandler>();
			loggerMock = new Mock<ILogger<TVMazeHttpClient>>();
			optionsMock = new Mock<IOptions<ScraperSettings>>();

			optionsMock.Setup(o => o.Value).Returns(new ScraperSettings
			{
				TVMazeApiBaseAddress = "http://localhost",
				ContactUrl = "ContactUrl",
				MaxNumberOfTVShowsPerPage = 1,
				NumberOfRetryAttempts = 1,
				ProceedWhereLastLeftOff = true,
				SecondsWaitTimeBeforeNextRetryAttempt = 1
			});
		}

		private TVMazeHttpClient CreateSut()
		{
			var httpClient = new HttpClient(httpMessageHandlerMock.Object);
			return new TVMazeHttpClient(httpClient, loggerMock.Object, optionsMock.Object);
		}

		#region Constructor
		[Fact]
		public void Constructor_Throws_ArgumentNullException_When_HttpClient_Is_Null()
		{
			// Arrange, Act & Assert
			Assert.Throws<ArgumentNullException>(() => new TVMazeHttpClient(null, loggerMock.Object, optionsMock.Object));
		}

		[Fact]
		public void Constructor_Throws_ArgumentNullException_WhenLogger_Is_Null()
		{
			// Arrange, Act & Assert
			var httpClient = new HttpClient(httpMessageHandlerMock.Object);
			Assert.Throws<ArgumentNullException>(() => new TVMazeHttpClient(httpClient, null, optionsMock.Object));
		}

		[Fact]
		public void Constructor_Throws_ArgumentNullException_When_Settings_Are_Null()
		{
			// Arrange, Act & Assert
			var httpClient = new HttpClient(httpMessageHandlerMock.Object);
			Assert.Throws<ArgumentNullException>(() => new TVMazeHttpClient(httpClient, loggerMock.Object, null));
		}
		#endregion

		#region GetTVShowsByPage
		[Fact]
		public async Task GetTVShowsByPage_Throws_HttpRequestException_When_StatusCode_Is_Not_200()
		{
			// Arrange
			httpMessageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError
				});

			var sut = CreateSut();

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(() => sut.GetTVShowsByPage(It.IsAny<int>()));
		}

		[Fact]
		public async Task GetTVShowsByPage_Returns_TVShows_When_StatusCode_Is_200()
		{
			// Arrange
			var getTVShowsResponseString = "[{ \"id\":1, \"name\":\"Show1\", \"_embedded\":{\"cast\":[{\"person\":{\"id\":1,\"name\":\"Cast1\",\"birthday\":\"1979-07-17\"}}]}},\r\n{ \"id\":2, \"name\":\"Show2\", \"_embedded\":{\"cast\":[{\"person\":{\"id\":1,\"name\":\"Cast1\",\"birthday\":\"1979-07-17\"}}]}}]";
			httpMessageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(getTVShowsResponseString)
				});

			var sut = CreateSut();

			// Act
			var actual = await sut.GetTVShowsByPage(It.IsAny<int>());

			// Assert
			Assert.Equal(2, actual.Item1.Count());
		}
		#endregion

		#region GetTVShowById
		[Fact]
		public async Task GetTVShowById_Throws_HttpRequestException_When_StatusCode_Is_Not_200()
		{
			// Arrange
			httpMessageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError,
				});

			var sut = CreateSut();

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(() => sut.GetTVShowById(It.IsAny<int>()));
		}

		[Fact]
		public async Task GetTVShowById_Returns_TVShow_When_StatusCode_Is_200()
		{
			// Arrange
			var getTVShowsResponseString = "{ \"id\":1, \"name\":\"Show1\", \"_embedded\":{\"cast\":[{\"person\":{\"id\":1,\"name\":\"Cast1\",\"birthday\":\"1979-07-17\"}}]}}";
			httpMessageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(getTVShowsResponseString)
				});

			var sut = CreateSut();

			// Act
			var actual = await sut.GetTVShowById(It.IsAny<int>());

			// Assert
			Assert.Equal(1, actual.Id);
			Assert.Equal("Show1", actual.Name);
			#endregion
		}
	}
}
