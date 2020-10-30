using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RTL.TVShows.Infrastructure.Interfaces;
using RTL.TVShows.Infrastructure.Models;
using RTL.TVShows.Infrastructure.Settings;
using RTL.TVShows.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RTL.TVShows.Service
{
	public class ScraperService : IScraperService
    {
        private readonly ILogger<ScraperService> logger;
        private readonly ITVShowsRepository tvShowRepository;
        private readonly ITVMazeHttpClient tvMazeHttpClient;
        private readonly ScraperSettings settings;

        public ScraperService(ILogger<ScraperService> logger, ITVShowsRepository tvShowRepository, ITVMazeHttpClient tvMazeHttpClient, IOptions<ScraperSettings> options)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.tvShowRepository = tvShowRepository ?? throw new ArgumentNullException(nameof(tvShowRepository));
            this.tvMazeHttpClient = tvMazeHttpClient ?? throw new ArgumentNullException(nameof(tvMazeHttpClient));
            settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task ScrapeTVShows()
        {
            var page = await GetStartPage();

            logger.LogInformation("Starting scraping!");
            while (true)
            {
                (IEnumerable<TVShowModel> tvShows, HttpStatusCode httpStatusCode) = await tvMazeHttpClient.GetTVShowsByPage(page);

                if (httpStatusCode == HttpStatusCode.NotFound)
                {
                    break;
                }

                foreach (var tvShow in tvShows)
                {
                    var tvShowWithCast = await tvMazeHttpClient.GetTVShowById(tvShow.Id);
                    tvShowRepository.UpsertTVShow(tvShowWithCast);
                }

                page++;
            }

            logger.LogInformation("Scraping completed!");
        }

        /// <summary>
        /// Gets the page number on which to start scraping, based on the logic provided by TV Maze: http://www.tvmaze.com/api#show-index.
        /// </summary>
        /// <returns>The page number on which to start scraping.</returns>
        private async Task<int> GetStartPage()
        {
            var page = 1;
            if(settings.ProceedWhereLastLeftOff)
            {
                var id = await tvShowRepository.GetIdByLastModifiedTVShow();
                if (id != null)
                {
                    page = decimal.ToInt32(Math.Floor(id.Value / settings.MaxNumberOfTVShowsPerPage));
                    logger.LogInformation($"Continue scraping from page {page}!");
                }
            }

            return page;
        }
    }
}
