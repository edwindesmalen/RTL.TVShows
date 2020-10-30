using Newtonsoft.Json;

namespace RTL.TVShows.Infrastructure.Models
{
	public class EmbeddedModel
    {
        [JsonProperty("cast")]
        public CastModel[] Cast { get; set; }
    }
}
