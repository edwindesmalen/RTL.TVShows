using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RTL.TVShows.Infrastructure.Interfaces;
using RTL.TVShows.Infrastructure.Models;
using RTL.TVShows.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RTL.TVShows.Infrastructure.HttpClients
{
    public class TVMazeHttpClient : ITVMazeHttpClient
    {
        private readonly HttpClient client;
        private readonly ILogger<TVMazeHttpClient> logger;
        private readonly ScraperSettings settings;

        public TVMazeHttpClient(HttpClient client, ILogger<TVMazeHttpClient> logger, IOptions<ScraperSettings> options)
        {
            settings = options?.Value ?? throw new ArgumentNullException(nameof(options));

            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(IEnumerable<TVShowModel>, HttpStatusCode)> GetTVShowsByPage(int page)
        {
            try
            {
                var requestUri = $"{settings.TVMazeApiBaseAddress}/shows?page={page}";

                (string contentString, HttpStatusCode httpStatusCode) = await GetContentAsync(requestUri);

                var tvShows = JsonConvert.DeserializeObject<IEnumerable<TVShowModel>>(contentString);

                return (tvShows, httpStatusCode);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<TVShowModel> GetTVShowById(int tvShowId)
        {
            try
            {
                var requestUri = $"{settings.TVMazeApiBaseAddress}/shows/{tvShowId}?embed[]=cast";

                (string contentString, HttpStatusCode httpStatusCode) = await GetContentAsync(requestUri);

                return JsonConvert.DeserializeObject<TVShowModel>(contentString);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<(string, HttpStatusCode)> GetContentAsync(string requestUri)
        {
            var response = await client.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode &&
                response.StatusCode != HttpStatusCode.NotFound &&
                (int)response.StatusCode != 429) // Note: When the solution is upgraded to .NET Core 3.1 and .NET Standard 2.1 update this line to use HttpStatusCode.TooManyRequests.
            {
                throw new HttpRequestException("Failed to retrieve data from TV Maze API.");
            }

            var contentString = await response.Content.ReadAsStringAsync();

            return (contentString, response.StatusCode);
        }
    }
}
