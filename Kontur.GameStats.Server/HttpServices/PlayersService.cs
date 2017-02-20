using System.Linq;
using GL.HttpServer.Attributes;
using GL.HttpServer.Cache;
using GL.HttpServer.HttpServices;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.DTO.CacheInfo;

namespace Kontur.GameStats.Server.HttpServices
{
    [HttpService("players")]
    public class PlayersService : IHttpService
    {
        /// <summary>
        ///     Получить статистику игрока
        /// </summary>
        [GetOperation("stats", "/<name>/stats")]
        public PlayerStatsDto GetPlayerStats(string name)
        {
            PlayerStatsTempInfo playerStats;
            var model = new PlayerStatsDto();
            if (MemoryCache.Global.TryGetValue(name, out playerStats))
            {
                model.Name = name;
                model.AverageScoreboardPercent = playerStats.Score / playerStats.TotalMatchesPlayed;
                model.MaximumMatchesPerDay = playerStats.MatchesPerDay.Max(a => a.Value);
                model.AverageMatchesPerDay = playerStats.MatchesPerDay.Average(a => a.Value);
                model.FavoriteGameMode = playerStats.GetFavoriteGameMode();
                model.FavoriteServer = playerStats.GetFavoriteServer();
                model.TotalMatchesWon = playerStats.TotalMatchesWon;
                model.TotalMatchesPlayed = playerStats.TotalMatchesPlayed;
                model.LastMatchPlayed = playerStats.LastMatchPlayed;
                model.UniqueServers = playerStats.Servers.Count;
                model.KillToDeathRatio = (double)playerStats.Kills / playerStats.Deaths;
                return model;
            }
            model.StatusCode = 404;
            return model;
        }
    }
}