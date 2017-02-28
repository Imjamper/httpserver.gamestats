using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.Attributes;
using GL.HttpServer.Cache;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Context;
using GL.HttpServer.Database;
using GL.HttpServer.Extensions;
using Kontur.GameStats.Server.CacheLoaders;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.Dto.CacheInfo;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.DTO.CacheInfo;

namespace Kontur.GameStats.Server.HttpServices
{
    [HttpService("reports")]
    public class ReportsService : IHttpService
    {
        [GetOperation("recent-matches", "/recent-matches[/<count>]")]
        public JsonList<MatchDto> GetRecentMatches([Bind("[/{count}]")]int count)
        {
            var rowsCount = count == 0 ? 5 : (count > 50 ? 50 : count);
            var model = new JsonList<MatchDto>();
            var recentMatches = MemoryCache.Cache<RecentMatchesTempInfo>().Get(RecentMatchesCacheLoader.RecentMatchesUid);
            if (recentMatches != null && recentMatches.Count > 0)
            {
                model = recentMatches.Take(rowsCount);
            }
            else
            {
                model.StatusCode = 404;
            }
            return model;
        }

        [GetOperation("best-players", "/best-players[/<count>]")]
        public JsonList<ShortPlayerStatsDto> GetBestPlayers([Bind("[/{count}]")]int count)
        {
            var rowsCount = count == 0 ? 5 : (count > 50 ? 50 : count);
            var model = new JsonList<ShortPlayerStatsDto>();
            var playerStats = MemoryCache.Cache<PlayerStatsTempInfo>().GetAll();
            if (playerStats != null)
            {
                foreach (var playerStat in playerStats)
                {
                    if (playerStat.Deaths > 0 && playerStat.TotalMatchesPlayed >= 10)
                        model.Add(new ShortPlayerStatsDto { Name = playerStat.Name, KillToDeathRatio = (double)playerStat.Kills / playerStat.Deaths });
                }

                return model.OrderByDescending(a => a.KillToDeathRatio).Take(rowsCount).ToJsonList();
            }
            model.StatusCode = 404;
            return model;
        }

        [GetOperation("popular-servers", "/popular-servers[/<count>]")]
        public JsonList<ShortServerStatsDto> GetPopularServers([Bind("[/{count}]")]int count)
        {
            using (var unit = new UnitOfWork())
            {
                var rowsCount = count == 0 ? 5 : (count > 50 ? 50 : count);
                var servers = unit.Repository<Entities.Server>().FindAll();
                var stats = new List<ShortServerStatsDto>();
                foreach (var server in servers)
                {
                    var serverStats = MemoryCache.Cache<ServerStatsTempInfo>().Get(server.Endpoint);
                    if (serverStats != null)
                    {
                        stats.Add(new ShortServerStatsDto
                        {
                            Endpoint = server.Endpoint,
                            Name = server.Info.Name,
                            AverageMatchesPerDay = serverStats.MatchesPerDay.Average(a => a.Value)
                        });
                    }
                }

                return stats.OrderByDescending(a => a.AverageMatchesPerDay).Take(rowsCount).ToJsonList();
            }
        }
    }
}
