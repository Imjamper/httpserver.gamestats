using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.GameStats.Server.Dto.CacheInfo;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.DTO.CacheInfo
{
    public class PlayerStatsTempInfo
    {
        public PlayerStatsTempInfo()
        {
            Servers = new List<CountItem>();
            GameModes = new List<CountItem>();
            MatchesPerDay = new Dictionary<string, int>();
        }

        public PlayerStatsTempInfo(string name, Match match)
        {
            Servers = new List<CountItem>();
            GameModes = new List<CountItem>();
            MatchesPerDay = new Dictionary<string, int>();
            Name = name;
            Update(match);
        }

        public string Name { get; set; }
        public int TotalMatchesPlayed { get; set; }
        public int TotalMatchesWon { get; set; }
        public List<CountItem> Servers { get; set; }
        public List<CountItem> GameModes { get; set; }
        public double Score { get; set; }
        public DateTime? LastMatchPlayed { get; set; }
        public Dictionary<string, int> MatchesPerDay { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }

        public void Update(Match match)
        {
            TotalMatchesPlayed++;
            var wonPlayer = match.Results.ScoreBoard.FirstOrDefault();
            var player = match.Results.ScoreBoard.FirstOrDefault(a => a.Name == Name);
            if (player != null)
            {
                if (wonPlayer != null && wonPlayer.Name == Name)
                    TotalMatchesWon++;

                if (Servers.Any(a => a.Value == match.Server))
                {
                    var item = Servers.FirstOrDefault(a => a.Value == match.Server);
                    if (item != null) item.Count++;
                }
                else Servers.Add(new CountItem {Count = 1, Value = match.Server});

                if (GameModes.Any(a => a.Value == match.Results.GameMode))
                {
                    var item = GameModes.FirstOrDefault(a => a.Value == match.Results.GameMode);
                    if (item != null) item.Count++;
                }
                else GameModes.Add(new CountItem {Count = 1,Value = match.Results.GameMode});

                Score += GetPlayerMatchScore(match.Results.ScoreBoard);
                if (LastMatchPlayed == null)
                    LastMatchPlayed = match.TimeStamp;
                else if (LastMatchPlayed < match.TimeStamp)
                    LastMatchPlayed = match.TimeStamp;

                if (MatchesPerDay.ContainsKey(match.TimeStamp.Value.ToString("yyyy-MM-dd")))
                {
                    MatchesPerDay[match.TimeStamp.Value.ToString("yyyy-MM-dd")]++;
                }
                else MatchesPerDay.Add(match.TimeStamp.Value.ToString("yyyy-MM-dd"), 1);

                Kills += player.Kills;
                Deaths += player.Deaths;
            }
        }

        public string GetFavoriteGameMode()
        {
            var max = GameModes.Max(a => a.Count);
            var countItem = GameModes.FirstOrDefault(a => a.Count == max);
            if (countItem != null)
                return countItem.Value;
            return String.Empty;
        }

        public string GetFavoriteServer()
        {
            var max = Servers.Max(a => a.Count);
            var countItem = Servers.FirstOrDefault(a => a.Count == max);
            if (countItem != null)
                return countItem.Value;
            return String.Empty;
        }

        private double GetPlayerMatchScore(List<PlayerScore> players)
        {
            var playersCount = players.Count;
            var player = players.FirstOrDefault(a => a.Name == Name);
            if (player != null)
            {
                var indexOfPlayer = players.IndexOf(player);
                var playersBelow = playersCount - (indexOfPlayer + 1);
                if (playersBelow == 0) return 0;
                return (playersCount - 1) / playersBelow * 100;
            }
            return 0;
        }
    }
}
