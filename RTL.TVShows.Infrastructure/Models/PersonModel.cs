using Newtonsoft.Json;
using System;

namespace RTL.TVShows.Infrastructure.Models
{
	public class PersonModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthday")]
        public DateTime? Birthday { get; set; }
    }
}
