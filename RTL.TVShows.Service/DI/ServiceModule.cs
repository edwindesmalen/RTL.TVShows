using Microsoft.Extensions.DependencyInjection;
using RTL.TVShows.Service.Interfaces;

namespace RTL.TVShows.Service.DI
{
    public static class ServiceModule
    {
        public static void RegisterServiceDependencies(this IServiceCollection services)
        {
            #region Services
            services.AddScoped<IScraperService, ScraperService>();
            services.AddScoped<ITVShowsService, TVShowsService>();
            #endregion
        }
    }
}
