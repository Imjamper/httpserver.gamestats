using System;
using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.Attributes;
using GL.HttpServer.Cache;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Context;
using GL.HttpServer.Database;
using GL.HttpServer.Extensions;
using Kontur.GameStats.Server.Dto;
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
            var model1 = new JsonList<MatchDto>();
            var model = new MatchDto();
            model.Server = "192.168.1.1-8080";
            model.TimeStamp = DateTime.Now;
            var model12 = new MatchResultDto();
            model12.Map = "DM-HelloWorld122";
            model12.GameMode = "Single";
            model12.FragLimit = 20;
            model12.TimeLimit = 20;
            model12.TimeElapsed = 12.345678;
            model12.ScoreBoard.Add(new PlayerScoreDto { Name = "Player16", Deaths = 2, Kills = 10, Frags = 14 });
            model12.ScoreBoard.Add(new PlayerScoreDto() { Name = "Player22", Deaths = 21, Kills = 4, Frags = 3 });
            model.Results = model12;
            model1.Add(model);
            return model1;
        }

        [GetOperation("best-players", "/best-players[/<count>]")]
        public JsonList<ShortPlayerStatsDto> GetBestPlayers([Bind("[/{count}]")]int count)
        {
            var model = new JsonList<ShortPlayerStatsDto>();
            model.Add(new ShortPlayerStatsDto() {Name = "Player1", KillToDeathRatio = 12.123123 });
            model.Add(new ShortPlayerStatsDto() { Name = "Player11", KillToDeathRatio = 3.1123123432 });
            return model;
        }

        [GetOperation("popular-servers", "/popular-servers[/<count>]")]
        public JsonList<ShortServerStatsDto> GetPopularServers([Bind("[/{count}]")]int count)
        {
            using (var unit = new UnitOfWork(true))
            {
                var rowsCount = count == 0 ? 5 : (count > 50 ? 50 : count);
                var servers = unit.Repository<Entities.Server>().FindAll();
                var stats = new List<ShortServerStatsDto>();
                foreach (var server in servers)
                {
                    var serverStats = MemoryCache.Global.Get<ServerStatsTempInfo>(server.Endpoint);
                    if (serverStats != null)
                    {
                        stats.Add(new ShortServerStatsDto()
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
