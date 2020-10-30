namespace RTL.TVShows.Infrastructure.Settings
{
    public class ScraperSettings
    {
        public bool ProceedWhereLastLeftOff { get; set; }
        public string TVMazeApiBaseAddress { get; set; }
        public decimal MaxNumberOfTVShowsPerPage { get; set; }
        public int NumberOfRetryAttempts { get; set; }
        public int SecondsWaitTimeBeforeNextRetryAttempt { get; set; }
        public string ContactUrl { get; set; }
    }
}
