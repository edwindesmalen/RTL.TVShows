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
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TVShow>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 1)
        {
            var tvShows = await tVShowService.GetTVShows(pageNumber, pageSize);
            if(tvShows == null || !tvShows.Any())
            {
                return NotFound();
            }

            return Ok(tvShows);
        }
    }
}
