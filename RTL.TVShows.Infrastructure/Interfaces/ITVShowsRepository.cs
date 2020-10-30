using RTL.TVShows.Domain;
using RTL.TVShows.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.TVShows.Infrastructure.Interfaces
{
	public interface ITVShowsRepository
    {
        Task<IEnumerable<TVShow>> GetTVShowsAsync();

        void UpsertTVShow(TVShowModel tvShow);

        Task<int?> GetIdByLastModifiedTVShow();
    }
}
