using RTL.TVShows.Infrastructure.Mappers;
using RTL.TVShows.Infrastructure.Models;
using System;
using Xunit;

namespace RTL.TVShows.Infrastructure.Tests.Mappers
{
	public class CastMapperTests
	{
		private CastMapper CreateSut()
		{
			return new CastMapper();
		}

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
		public void Map_Throws_ArgumentNullExecption_When_Person_Is_Null()
		{
			// Arrange
			var instance = new CastModel { Person = null };

			var sut = CreateSut();

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => sut.Map(instance));
		}

		[Fact]
		public void Map_Maps_All_Properties_When_All_Needed_Properties_Arent_Null_Or_Empty()
		{
			// Arrange
			var expectedId = 1;
			var expectedName = "expectedName";
			var expectedBirthday = new DateTime(2020, 10, 30);

			var instance = new CastModel
			{
				Person = new PersonModel
				{
					Id = expectedId,
					Name = expectedName,
					Birthday = expectedBirthday
				}
			};

			var sut = CreateSut();

			// Act
			var actual = sut.Map(instance);

			// Assert
			Assert.Equal(expectedId, actual.Id);
			Assert.Equal(expectedName, actual.Name);
			Assert.Equal(expectedBirthday, actual.Birthday);
		}
		#endregion
	}
}
