using System.Collections.Generic;
using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.Models
{
    public class ServerStatsInfo : JsonResponse
    {
        public ServerStatsInfo()
        {
            Top5GameModes = new List<string>();
            Top5Maps = new List<string>();
        }

        public int TotalMatchesPlayed { get; set; }

        public int MaximumMatchesPerDay { get; set; }

        public double AverageMatchesPerDay { get; set; }

        public int MaximumPopulation { get; set; }

        public double AveragePopulation { get; set; }

        public List<string> Top5GameModes { get; set; }

        public List<string> Top5Maps { get; set; }
    }
}