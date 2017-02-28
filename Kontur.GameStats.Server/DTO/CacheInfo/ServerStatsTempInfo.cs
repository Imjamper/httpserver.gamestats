using System.Collections.Generic;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.DTO.CacheInfo
{
    public class ServerStatsTempInfo
    {
        public ServerStatsTempInfo()
        {
            MatchesPerDay = new Dictionary<string, int>();
            Maps = new Dictionary<string, int>();
            GameModes = new Dictionary<string, int>();
        }

        public ServerStatsTempInfo(Match match)
        {
            MatchesPerDay = new Dictionary<string, int>();
            Maps = new Dictionary<string, int>();
            GameModes = new Dictionary<string, int>();
            Update(match);
        }
        public int TotalMatchesPlayed { get; set; }
        public Dictionary<string, int> MatchesPerDay { get; set; }
        public int Population { get; set; }
        public int MaximumPopulation { get; set; }
        public Dictionary<string, int> Maps { get; set; }
        public Dictionary<string, int> GameModes { get; set; }

        public void Update(Match match)
        {
            TotalMatchesPlayed++;
            if (MatchesPerDay.ContainsKey(match.TimeStamp.ToString("yyyy-MM-dd")))
            {
                MatchesPerDay[match.TimeStamp.ToString("yyyy-MM-dd")]++;
            }
            else MatchesPerDay.Add(match.TimeStamp.ToString("yyyy-MM-dd"), 1);
            if (GameModes.ContainsKey(match.Results.GameMode))
            {
                GameModes[match.Results.GameMode]++;
            }
            else GameModes.Add(match.Results.GameMode, 1);
            if (Maps.ContainsKey(match.Results.Map))
            {
                Maps[match.Results.Map]++;
            }
            else Maps.Add(match.Results.Map, 1);
            Population += match.Results.ScoreBoard.Count;
            if (MaximumPopulation < match.Results.ScoreBoard.Count)
                MaximumPopulation = match.Results.ScoreBoard.Count;
        }
    }
}
