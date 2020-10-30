using RTL.TVShows.Domain;
using RTL.TVShows.Infrastructure.Interfaces;
using RTL.TVShows.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.TVShows.Service
{
	public class TVShowsService : ITVShowsService
	{
		private readonly ITVShowsRepository tvShowRepository;

		public TVShowsService(ITVShowsRepository tvShowRepository)
		{
			this.tvShowRepository = tvShowRepository ?? throw new ArgumentNullException(nameof(tvShowRepository));
		}

		public async Task<IEnumerable<TVShow>> GetTVShows(int pageNumber, int pageSize)
		{
			IEnumerable<TVShow> result = null;
			if (pageNumber >= 1 && pageSize >= 1)
			{
				var tvShows = await tvShowRepository.GetTVShowsAsync();

				result = tvShows
					.Skip(pageSize * pageNumber)
					.Take(pageSize)
					.Select(t =>
					{
						t.Cast = t.Cast.OrderByDescending(c => c.Birthday);

						return t;
					});
			}

			return result;
		}
	}
}
