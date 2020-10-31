using Microsoft.AspNetCore.Mvc;
using RTL.TVShows.Domain;
using RTL.TVShows.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.TVShows.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TVShowsController : ControllerBase
    {
        private readonly ITVShowsService tVShowService;

        public TVShowsController(ITVShowsService tVShowService)
        {
            this.tVShowService = tVShowService ?? throw new ArgumentNullException(nameof(tVShowService));
        }

        // GET api/tvshows?pagenumber=3&pagesize=10
        /// <summary>
        /// Get TV Shows by page number and page size.
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paginated list of TV Shows containging the id of the TV show and a list of all the cast that are playing in that TV show. The list of the cast is ordered by birthday descending.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TVShow>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 1)
        {
            var tvShows = await tVShowService.GetTVShowsAsync(pageNumber, pageSize);
            if(tvShows == null || !tvShows.Any())
            {
                return NotFound();
            }

            return Ok(tvShows);
        }
    }
}
