using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GL.HttpServer.Enums;
using GL.HttpServer.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.DTO.CacheInfo;
using Kontur.GameStats.Server.Entities;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    [TestFixture]
    public class ReportsServiceTests : ServiceTests
    {
        [Test, Order(1)]
        public void RecentMatches_PutNewMatches_GetSameMatchesInfo()
        {
            var matches = PutMatchesInfo(4);
            var matchesJson = JsonConvert.SerializeObject(matches);
            var response = ExecuteUrl("reports/recent-matches[/4]", null, MethodType.GET);

            var getRecentMatches = JsonConvert.DeserializeObject<List<MatchDto>>(response.JsonString);

            Assert.AreEqual(getRecentMatches.Count, 4);
            Assert.AreEqual(response.JsonString, matchesJson);
        }

        [Test, Order(2)]
        public void GetBestPlayers_PutNewMatches_GetValidBestPlayersInfo()
        {
            var serverBestPlayers = GetServer("localhost-7575", "BestPlayersTestServer");
            var matches = new List<MatchDto>();
            var advertisePut = ExecuteUrl($"servers/{serverBestPlayers.Endpoint}/info", serverBestPlayers.Info, MethodType.PUT);
            Assert.AreEqual(advertisePut.StatusCode, "OK");
            Assert.IsNull(advertisePut.ErrorMessage);
            for (int i = 0; i < 15; i++)
            {
                var match = new MatchDto();
                match.Results = new MatchResultDto();
                match.Results.FragLimit = 20;
                match.Results.GameMode = "SINGLE";
                match.Results.Map = "TOP";
                match.Results.TimeElapsed = 25.34;
                match.Results.TimeLimit = 40;
                match.Results.ScoreBoard.Add(new PlayerScoreDto {Deaths = 10, Frags = 10, Kills = 20, Name = "BestTestPlayer"});
                match.TimeStamp = DateTimeOffset.UtcNow;
                var date = match.TimeStamp.UtcDateTime.ToString(UtcFormat);
                var putMatchResponse = ExecuteUrl($"servers/{serverBestPlayers.Endpoint}/matches/{date}", match.Results, MethodType.PUT);
                Assert.AreEqual(putMatchResponse.StatusCode, "OK");
                Assert.IsNull(putMatchResponse.ErrorMessage);
                matches.Add(match);    
            }
            var playerTempInfos = new List<PlayerStatsTempInfo>();
            foreach (var matchDto in matches)
            {
                var players = matchDto.Results.ScoreBoard;
                foreach (var playerScore in players)
                {
                    var playerStats = playerTempInfos.FirstOrDefault(a => a.Name == playerScore.Name);
                    if (playerStats != null)
                        playerStats.Update(matchDto.ToEntity<Match>());
                    else
                    {
                        playerStats = new PlayerStatsTempInfo(playerScore.Name, matchDto.ToEntity<Match>());
                        playerTempInfos.Add(playerStats);
                    }
                }
            }
            var playerStatsInfo = playerTempInfos
                .Where(a => a.TotalMatchesPlayed >= 10 && a.Deaths > 0)
                .Select(b => new ShortPlayerStatsDto {KillToDeathRatio = b.Kills / (double)b.Deaths, Name = b.Name})
                .OrderByDescending(a => a.KillToDeathRatio)
                .Take(50).ToList();

            var getResponse = ExecuteUrl("reports/best-players[/50]", null, MethodType.GET);

            var getBestPlayers = JsonConvert.DeserializeObject<List<ShortPlayerStatsDto>>(getResponse.JsonString);

            Assert.AreEqual(getBestPlayers.Count, playerStatsInfo.Count);
            foreach (var shortPlayerStatsDto in getBestPlayers)
            {
                var index = getBestPlayers.IndexOf(shortPlayerStatsDto);
                var inputPlayer = playerStatsInfo.ElementAtOrDefault(index);
                if (inputPlayer != null)
                {
                    Assert.IsNotNull(inputPlayer);
                    Assert.AreEqual(shortPlayerStatsDto.Name, inputPlayer.Name);
                    Assert.AreEqual(shortPlayerStatsDto.KillToDeathRatio, inputPlayer.KillToDeathRatio);
                }
            }
        }

        [Test, Order(3)]
        public void GetPopularServers_PutNewMatches_GetPopularServerInfo()
        {
            var testServerOne = GetServer("localhost-1111", "TestServer1111");
            PutMatchesInfo(5, testServerOne, true);
            var testServerTwo = GetServer("localhost-2222", "TestServer2222");
            PutMatchesInfo(5, testServerTwo, true);
            var testServerThree = GetServer("localhost-3333", "TestServer3333");
            PutMatchesInfo(5, testServerThree, true);

            var getResponse = ExecuteUrl("reports/popular-servers[/3]", null, MethodType.GET);

            var getPopularServers = JsonConvert.DeserializeObject<List<ShortServerStatsDto>>(getResponse.JsonString);

            Assert.IsTrue(getPopularServers.Count > 0 && getPopularServers.Count <= 3);
        }
    }
}
