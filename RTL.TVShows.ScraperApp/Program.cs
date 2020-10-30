using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using RTL.TVShows.Infrastructure.DI;
using RTL.TVShows.Service.DI;
using RTL.TVShows.Service.Interfaces;
using System.Threading.Tasks;

namespace RTL.TVShows.ScraperApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var services = new ServiceCollection();
            services.AddLogging(configure => configure
                .AddConsole()
                .AddEventLog(new EventLogSettings
                {
                    LogName = config.GetValue<string>("Logging:EventLog:LogName"),
                    SourceName = config.GetValue<string>("Logging:EventLog:SourceName")
                }));

            services.RegisterInfrastructureDependencies(config);
            services.RegisterServiceDependencies();

            var provider = services.BuildServiceProvider();
            var scraperService = provider.GetRequiredService<IScraperService>();

            await scraperService.ScrapeTVShows();
        }
    }
}
