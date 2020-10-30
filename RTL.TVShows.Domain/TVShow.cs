using System.Collections.Generic;

namespace RTL.TVShows.Domain
{
    public class TVShow
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Cast> Cast { get; set; }
    }
}
