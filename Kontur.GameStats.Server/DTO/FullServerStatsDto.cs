using System.Collections.Generic;

namespace Kontur.GameStats.Server.DTO
{
    public class FullServerStatsDto : ShortServerStats
    {
        public FullServerStatsDto()
        {
            Top5GameModes = new List<string>();
            Top5Maps = new List<string>();
        }

        public int TotalMatchesPlayed { get; set; }

        public int MaximumMatchesPerDay { get; set; }

        public int MaximumPopulation { get; set; }

        public double AveragePopulation { get; set; }

        public List<string> Top5GameModes { get; set; }

        public List<string> Top5Maps { get; set; }
    }
}