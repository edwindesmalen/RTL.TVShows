using RTL.TVShows.Domain;
using RTL.TVShows.Extensions.Interfaces;
using RTL.TVShows.Infrastructure.Models;
using System;
using System.Linq;

namespace RTL.TVShows.Infrastructure.Mappers
{
	public class TVShowMapper : IMapper<TVShowModel, TVShow>
	{
		private readonly IMapper<CastModel, Cast> castMapper;

		public TVShowMapper(IMapper<CastModel, Cast> castMapper)
		{
			this.castMapper = castMapper ?? throw new ArgumentNullException(nameof(castMapper));
		}

		public TVShow Map(TVShowModel instance)
		{
			if (instance == null) throw new ArgumentNullException(nameof(instance));

			return new TVShow
			{
				Id = instance.Id,
				Name = instance.Name,
				Cast = instance.Embedded?.Cast?.Select(c => castMapper.Map(c))
			};
		}
	}
}
