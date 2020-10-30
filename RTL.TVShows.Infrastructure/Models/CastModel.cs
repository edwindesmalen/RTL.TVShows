using Newtonsoft.Json;

namespace RTL.TVShows.Infrastructure.Models
{
	public class CastModel
    {
        [JsonProperty("person")]
        public PersonModel Person { get; set; }
    }
}
