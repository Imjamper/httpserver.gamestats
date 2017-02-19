using System;
using Kontur.GameStats.Server.DTO;

namespace Kontur.GameStats.Server.Dto
{
    public class PlayerStatsDto : ShortPlayerStatsDto
    {
        public PlayerStatsDto()
        {
            
        }
        public int TotalMatchesPlayed { get; set; }
        public int TotalMatchesWon { get; set; }
        public string FavoriteServer { get; set; }
        public int UniqueServers { get; set; }
        public string FavoriteGameMode { get; set; }
        public double AverageScoreboardPercent { get; set; }
        public int MaximumMatchesPerDay { get; set; }
        public double AverageMatchesPerDay { get; set; }
        public DateTime? LastMatchPlayed { get; set; }
    }
}