using System;
using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.DTO.CacheInfo;
using Kontur.GameStats.Server.Entities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    [TestFixture]
    public class PlayersServiceTests : ServiceTests
    {
        [Test]
        public void GetPlayerStats_PutMatches_GetValidPlayerStats()
        {
            var playerStatsServer = GetServer("localhost-6756", "PlayerStatsServer");
            var matches = new List<MatchDto>();
            var advertisePut = ExecuteUrl($"servers/{playerStatsServer.Endpoint}/info", playerStatsServer.Info, MethodType.PUT);
            Assert.AreEqual(advertisePut.StatusCode, "OK");
            Assert.IsNull(advertisePut.ErrorMessage);
            for (int i = 0; i < 50; i++)
            {
                var match = new MatchDto();
                match.Results = new MatchResultDto();
                match.Results.FragLimit = 20;
                match.Results.GameMode = "SINGLE";
                match.Results.Map = "TOP";
                match.Results.TimeElapsed = 25.34;
                match.Results.TimeLimit = 40;
                match.Results.ScoreBoard.Add(new PlayerScoreDto { Deaths = 10, Frags = 10, Kills = 64, Name = "StatsPlayerOne" });
                match.Results.ScoreBoard.Add(new PlayerScoreDto { Deaths = 10, Frags = 10, Kills = 40, Name = "StatsPlayerTwo" });
                match.Results.ScoreBoard.Add(new PlayerScoreDto { Deaths = 10, Frags = 12, Kills = 30, Name = "StatsPlayerThree" });
                match.Results.ScoreBoard.Add(new PlayerScoreDto { Deaths = 10, Frags = 10, Kills = 23, Name = "StatsPlayerFour" });
                match.TimeStamp = DateTimeOffset.UtcNow;
                var date = match.TimeStamp.UtcDateTime.ToString(UtcFormat);
                var putMatchResponse = ExecuteUrl($"servers/{playerStatsServer.Endpoint}/matches/{date}", match.Results, MethodType.PUT);
                Assert.AreEqual(putMatchResponse.StatusCode, "OK");
                Assert.IsNull(putMatchResponse.ErrorMessage);
                matches.Add(match);
            }
            var playerTempInfos = new List<PlayerStatsTempInfo>();
            foreach (var matchDto in matches)
            {
                var playerStats = playerTempInfos.FirstOrDefault(a => a.Name == "StatsPlayerOne");
                if (playerStats != null)
                    playerStats.Update(matchDto.ToEntity<Match>());
                else
                {
                    playerStats = new PlayerStatsTempInfo("StatsPlayerOne", matchDto.ToEntity<Match>());
                    playerTempInfos.Add(playerStats);
                }
            }
            var playerOneStatsInfo = playerTempInfos.FirstOrDefault(a => a.Name == "StatsPlayerOne");

            var getResponse = ExecuteUrl("players/StatsPlayerOne/stats", null, MethodType.GET);

            var playerOneGetStats = JsonConvert.DeserializeObject<PlayerStatsDto>(getResponse.JsonString);

            Assert.NotNull(playerOneStatsInfo);
            Assert.NotNull(playerOneGetStats);
            Assert.AreEqual(playerOneGetStats.Name, playerOneStatsInfo.Name);
            Assert.AreEqual(playerOneGetStats.AverageMatchesPerDay, playerOneGetStats.AverageMatchesPerDay);
            Assert.AreEqual(playerOneGetStats.TotalMatchesPlayed, playerOneGetStats.TotalMatchesPlayed);
        }
    }
}
