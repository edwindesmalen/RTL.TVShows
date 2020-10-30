using RTL.TVShows.Domain;
using RTL.TVShows.Extensions.Interfaces;
using RTL.TVShows.Infrastructure.Models;
using System;

namespace RTL.TVShows.Infrastructure.Mappers
{
	public class CastMapper : IMapper<CastModel, Cast>
	{
		public Cast Map(CastModel instance)
		{
			if (instance == null) throw new ArgumentNullException(nameof(instance));
			if (instance.Person == null) throw new ArgumentNullException(nameof(instance.Person));

			return new Cast { 
				Id = instance.Person.Id,
				Name = instance.Person.Name,
				Birthday = instance.Person.Birthday
			};
		}
	}
}
