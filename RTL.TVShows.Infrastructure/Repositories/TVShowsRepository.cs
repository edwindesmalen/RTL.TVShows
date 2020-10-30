using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using RTL.TVShows.Domain;
using RTL.TVShows.Extensions.Interfaces;
using RTL.TVShows.Infrastructure.Interfaces;
using RTL.TVShows.Infrastructure.Models;
using RTL.TVShows.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTL.TVShows.Infrastructure.Repositories
{
    public class TVShowsRepository : ITVShowsRepository
    {
        private readonly TVShowsDatabaseSettings settings;
        private readonly IMapper<TVShowModel, TVShow> tvShowMapper;
        private readonly IMongoCollection<TVShowModel> tvShowsCollection;

        public TVShowsRepository(IOptions<TVShowsDatabaseSettings> options, IMapper<TVShowModel, TVShow> tvShowMapper)
        {
            settings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.tvShowMapper = tvShowMapper ?? throw new ArgumentNullException(nameof(tvShowMapper));

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            tvShowsCollection = database.GetCollection<TVShowModel>(settings.TVShowsCollectionName);
        }

        public async Task<IEnumerable<TVShow>> GetTVShowsAsync()
        {
            return (await GetTVShows()).Select(t => tvShowMapper.Map(t));
        }

        public void UpsertTVShow(TVShowModel tvShow)
        {
            tvShow.LastModificationDate = DateTime.Now;

            tvShowsCollection.ReplaceOne(
                new BsonDocument("_id", tvShow.Id),
                tvShow,
                new ReplaceOptions { IsUpsert = true });
        }

        public async Task<int?> GetIdByLastModifiedTVShow()
        {
            var tvShows = await GetTVShows();
            
            return tvShows.OrderByDescending(t => t.LastModificationDate).First()?.Id;
        }

        private async Task<IEnumerable<TVShowModel>> GetTVShows()
        {
            return (await tvShowsCollection.FindAsync(new BsonDocument())).ToList();
        }
    }
}
