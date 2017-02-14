using System;
using System.Collections.Generic;
using GL.HttpServer.Attributes;
using GL.HttpServer.HttpServices;
using GL.HttpServer.Context;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;

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
        public JsonList<ShortPlayerStats> GetBestPlayers([Bind("[/{count}]")]int count)
        {
            var model = new JsonList<ShortPlayerStats>();
            model.Add(new ShortPlayerStats {Name = "Player1", KillToDeathRatio = 12.123123 });
            model.Add(new ShortPlayerStats { Name = "Player11", KillToDeathRatio = 3.1123123432 });
            return model;
        }

        [GetOperation("popular-servers", "/popular-servers[/<count>]")]
        public JsonList<ShortServerStats> GetPopularServers([Bind("[/{count}]")]int count)
        {
            var model = new JsonList<ShortServerStats>();
            model.Add(new ShortServerStats() {Endpoint = "192.168.1.1-8080", Name = "MyServer1", AverageMatchesPerDay = 10.234234});
            model.Add(new ShortServerStats() { Endpoint = "192.168.1.14-8080", Name = "MyServer14", AverageMatchesPerDay = 2392.354 });
            return model;
        }
    }
}
