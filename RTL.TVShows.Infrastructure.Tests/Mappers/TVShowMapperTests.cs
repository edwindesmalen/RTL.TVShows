using Moq;
using RTL.TVShows.Domain;
using RTL.TVShows.Extensions.Interfaces;
using RTL.TVShows.Infrastructure.Mappers;
using RTL.TVShows.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RTL.TVShows.Infrastructure.Tests.Mappers
{
	public class TVShowMapperTests
	{
		private readonly Mock<IMapper<CastModel, Cast>> castMapperMock;

		public TVShowMapperTests()
		{
			castMapperMock = new Mock<IMapper<CastModel, Cast>>();
		}

		private TVShowMapper CreateSut()
		{
			return new TVShowMapper(castMapperMock.Object);
		}

		#region Constructor
		[Fact]
		public void Constructor_Throws_ArgumentNullException_When_CastMapper_Is_Null()
		{
			// Arrange, Act & Assert
			Assert.Throws<ArgumentNullException>(() => new TVShowMapper(null));
		}
		#endregion

		#region Map
		[Fact]
		public void Map_Throws_ArgumentNullExecption_When_Instance_Is_Null()
		{
			// Arrange
			var sut = CreateSut();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => sut.Map(null));
		}

		[Fact]
		public void Map_Maps_All_Properties_When_All_Needed_Properties_Arent_Null_Or_Empty()
		{
			// Arrange
			var expectedId = 1;
			var expectedName = "expectedName";
			var expectedLastModificationDate = new DateTime(2020, 10, 30);
			var expectedCast = new Cast();

			var instance = new TVShowModel
			{
				Id = expectedId,
				Name = expectedName,
				LastModificationDate = new DateTime(2020, 10, 30),
				Embedded = new EmbeddedModel
				{
					Cast = new List<CastModel> {
						new CastModel()
					}.ToArray()
				}
			};

			castMapperMock
				.Setup(mock => mock.Map(It.IsAny<CastModel>()))
				.Returns(expectedCast);


			var sut = CreateSut();

			// Act
			var actual = sut.Map(instance);

			// Assert
			Assert.Equal(expectedId, actual.Id);
			Assert.Equal(expectedName, actual.Name);
			Assert.NotNull(actual.Cast);
			Assert.Single(actual.Cast);
		}

		[Fact]
		public void Map_Maps_Cast_When_EmbeddedModel_Is_Null_Or_Empty()
		{
			// Arrange
			var instance = new TVShowModel
			{
				Embedded = null
			};

			var sut = CreateSut();

			// Act
			var actual = sut.Map(instance);

			// Assert
			Assert.Null(actual.Cast);
		}

		[Fact]
		public void Map_Maps_Cast_When_CastModel_Is_Null()
		{
			// Arrange
			var instance = new TVShowModel
			{
				Embedded = new EmbeddedModel
				{
					Cast = null
				}
			};

			var sut = CreateSut();

			// Act
			var actual = sut.Map(instance);

			// Assert
			Assert.Null(actual.Cast);
		}

		[Fact]
		public void Map_Maps_Cast_When_CastModel_Is_Empty()
		{
			// Arrange
			var instance = new TVShowModel
			{
				Embedded = new EmbeddedModel
				{
					Cast = new List<CastModel>().ToArray()
				}
			};

			var sut = CreateSut();

			// Act
			var actual = sut.Map(instance);

			// Assert
			Assert.Empty(actual.Cast);
		}
		#endregion
	}
}
