using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace RTL.TVShows.Infrastructure.Models
{

	public class TVShowModel
    {
        [BsonId]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("_embedded")]
        public EmbeddedModel Embedded { get; set; }

        public DateTime LastModificationDate { get; set; }
    }
}
