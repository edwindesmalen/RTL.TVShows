using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using RTL.TVShows.Domain;
using RTL.TVShows.Extensions.Interfaces;
using RTL.TVShows.Infrastructure.HttpClients;
using RTL.TVShows.Infrastructure.Interfaces;
using RTL.TVShows.Infrastructure.Mappers;
using RTL.TVShows.Infrastructure.Models;
using RTL.TVShows.Infrastructure.Repositories;
using RTL.TVShows.Infrastructure.Settings;
using System;
using System.Reflection;

namespace RTL.TVShows.Infrastructure.DI
{
    public static class InfrastructureModule
    {
        public static void RegisterInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region Settings
            services.Configure<ScraperSettings>(configuration.GetSection("Scraper"));
            services.Configure<TVShowsDatabaseSettings>(configuration.GetSection("TVShowsDatabase"));
            #endregion

            #region HttpClients
            var settings = configuration.GetSection("Scraper").Get<ScraperSettings>();
            services.AddHttpClient<ITVMazeHttpClient, TVMazeHttpClient>(c =>
                {
                    var assembly = Assembly.GetEntryAssembly().GetName();

                    c.BaseAddress = new Uri(settings.TVMazeApiBaseAddress);
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                    c.DefaultRequestHeaders.Add("User-Agent", $"{assembly.Name}/{assembly.Version} (In case of any problems, please reach out to us at {settings.ContactUrl})");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
                .OrResult(r => (int)r.StatusCode == 429) // Note: When the solution is upgraded to .NET Core 3.1 and .NET Standard 2.1 update this line to use HttpStatusCode.TooManyRequests.
                .WaitAndRetryAsync(settings.NumberOfRetryAttempts, retryAttempt => TimeSpan.FromSeconds(settings.SecondsWaitTimeBeforeNextRetryAttempt)));
            #endregion

            #region Mappers
            services.AddScoped<IMapper<TVShowModel, TVShow>, TVShowMapper>();
            services.AddScoped<IMapper<CastModel, Cast>, CastMapper>();
            #endregion

            #region Repositories
            services.AddScoped<ITVShowsRepository, TVShowsRepository>();
            #endregion
        }
    }
}
