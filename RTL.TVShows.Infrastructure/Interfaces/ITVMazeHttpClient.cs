using RTL.TVShows.Infrastructure.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RTL.TVShows.Infrastructure.Interfaces
{
    public interface ITVMazeHttpClient
    {
        Task<(IEnumerable<TVShowModel>, HttpStatusCode httpStatusCode)> GetTVShowsByPage(int page);
        Task<TVShowModel> GetTVShowById(int tvShowId);
    }
}
