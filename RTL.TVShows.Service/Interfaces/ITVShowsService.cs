using RTL.TVShows.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTL.TVShows.Service.Interfaces
{
    public interface ITVShowsService
    {
        Task<IEnumerable<TVShow>> GetTVShowsAsync(int pageNumber, int pageSize);
    }
}
