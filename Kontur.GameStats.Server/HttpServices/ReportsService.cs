using System;
using System.Collections.Generic;
using GL.HttpServer.Attributes;
using GL.HttpServer.HttpServices;
using Kontur.GameStats.Server.Models;
using GL.HttpServer.Context;
using Kontur.GameStats.Server.DTO;

namespace Kontur.GameStats.Server.HttpServices
{
    [HttpService("reports")]
    public class ReportsService : IHttpService
    {
        [GetOperation("recent-matches", "/recent-matches[/<count>]")]
        public RecentMatchesInfo GetRecentMatches([Bind("[/{count}]")]int count)
        {
            var model = new RecentMatchesInfo();
            model.Server = "192.168.1.1-8080";
            model.TimeStamp = DateTime.Now;
            var model1 = new MatchInfo();
            model1.Map = "DM-HelloWorld122";
            model1.GameMode = "Single";
            model1.FragLimit = 20;
            model1.TimeLimit = 20;
            model1.TimeElapsed = 12.345678;
            model1.ScoreBoard.Add(new PlayerScore { Name = "Player16", Deaths = 2, Kills = 10, Frags = 14 });
            model1.ScoreBoard.Add(new PlayerScore { Name = "Player22", Deaths = 21, Kills = 4, Frags = 3 });
            model.Results.Add(model1);
            var model2 = new MatchInfo();
            model2.Map = "DM-HelloWorld";
            model2.GameMode = "DM";
            model2.FragLimit = 20;
            model2.TimeLimit = 20;
            model2.TimeElapsed = 12.345678;
            model2.ScoreBoard.Add(new PlayerScore { Name = "Player188", Deaths = 2, Kills = 10, Frags = 14 });
            model2.ScoreBoard.Add(new PlayerScore { Name = "Player233", Deaths = 21, Kills = 4, Frags = 3 });
            model.Results.Add(model2);
            return model;
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
        public JsonList<ShortServerInfo> GetPopularServers([Bind("[/{count}]")]int count)
        {
            var model = new JsonList<ShortServerInfo>();
            model.Add(new ShortServerInfo() {Endpoint = "192.168.1.1-8080", Name = "MyServer1", AverageMatchesPerDay = 10.234234});
            model.Add(new ShortServerInfo() { Endpoint = "192.168.1.14-8080", Name = "MyServer14", AverageMatchesPerDay = 2392.354 });
            return model;
        }
    }
}
