using System;
using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.Enums;
using HttpClient;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    [TestFixture]
    public class ServerServiceTests : ServiceTest
    {
        [Test, Order(1)]
        public void PutServerInfo_PutNewServerInfo_GetSameServerInfo()
        {
            var putResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/info", GameServer.Info, MethodType.PUT);

            Assert.AreEqual(putResponse.StatusCode, "OK");
            Assert.IsNull(putResponse.ErrorMessage);

            var getResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/info", null, MethodType.GET);
            var getServerInfo = JsonConvert.DeserializeObject<ServerInfoDto>(getResponse.JsonString);

            Assert.AreEqual(getResponse.StatusCode, "OK");
            Assert.IsNull(getResponse.ErrorMessage);
            Assert.NotNull(getServerInfo);
            Assert.AreEqual(getServerInfo.Name, GameServer.Info.Name);
        }


        [Test, Order(2)]
        public void PutMatchInfo_PutNewMatch_GetSameMatchInfo()
        {
            var match = RandomGenerator.GetMatch();
            if (match.TimeStamp != null)
            {
                var date = match.TimeStamp.Value.UtcDateTime.ToString("o");
                var putMatchResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date}", match.Results, MethodType.PUT);
            
                Assert.AreEqual(putMatchResponse.StatusCode, "OK");
                Assert.IsNull(putMatchResponse.ErrorMessage);

                var getResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date}", null, MethodType.GET);
                var getMatchInfo = JsonConvert.DeserializeObject<MatchResultDto>(getResponse.JsonString);

                Assert.AreEqual(getResponse.StatusCode, "OK");
                Assert.IsNull(getResponse.ErrorMessage);
                Assert.NotNull(getMatchInfo);
                Assert.AreEqual(match.Results.ScoreBoard.Count, getMatchInfo.ScoreBoard.Count);
                Assert.AreEqual(match.Results.TimeLimit, getMatchInfo.TimeLimit);
            }
        }

        [Test, Order(3)]
        public void GetAllServersInfo_PutServersInfo_GetSameServersInfos()
        {
            var testServer1 = RandomGenerator.GetServer("localhost-8111", "TestServer1");
            var testServer2 = RandomGenerator.GetServer("localhost-8222", "TestServer2");

            var putResponse1 = ExecuteUrl($"servers/{testServer1.Endpoint}/info", testServer1.Info, MethodType.PUT);

            Assert.AreEqual(putResponse1.StatusCode, "OK");
            Assert.IsNull(putResponse1.ErrorMessage);

            var putResponse2 = ExecuteUrl($"servers/{testServer2.Endpoint}/info", testServer2.Info, MethodType.PUT);

            Assert.AreEqual(putResponse2.StatusCode, "OK");
            Assert.IsNull(putResponse2.ErrorMessage);

            var getResponse = ExecuteUrl("servers/info", null, MethodType.GET);
            var getServerInfos = JsonConvert.DeserializeObject<List<ServerDto>>(getResponse.JsonString);

            Assert.AreEqual(getResponse.StatusCode, "OK");
            Assert.IsNull(getResponse.ErrorMessage);
            Assert.NotNull(getServerInfos);
            Assert.AreEqual(getServerInfos.Count, 2);
        }

        [Test, Order(4)]
        public void GetServerStats_PutMatches_GetValidStats()
        {
            var match1 = RandomGenerator.GetMatch();
            if (match1.TimeStamp != null)
            {
                var date1 = match1.TimeStamp.Value.ToString(UtcFormat);
                var putMatchResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date1}", match1.Results, MethodType.PUT);

                Assert.AreEqual(putMatchResponse.StatusCode, "OK");
                Assert.IsNull(putMatchResponse.ErrorMessage);
            }

            var match2 = RandomGenerator.GetMatch();
            if (match2.TimeStamp != null)
            {
                var date2 = match2.TimeStamp.Value.ToString(UtcFormat);
                var putMatch1Response = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date2}", match2.Results, MethodType.PUT);

                Assert.AreEqual(putMatch1Response.StatusCode, "OK");
                Assert.IsNull(putMatch1Response.ErrorMessage);
            }

            var averagePopulation = (match1.Results.ScoreBoard.Count + match2.Results.ScoreBoard.Count) / (double)2;
            var maximumPopulation = Math.Max(match1.Results.ScoreBoard.Count, match2.Results.ScoreBoard.Count);
            var matches = new List<MatchDto> {match1, match2};
            var gameModes = new Dictionary<string, int>();
            var maps = new Dictionary<string, int>();
            foreach (var match in matches)
            {
                if (gameModes.ContainsKey(match.Results.GameMode))
                {
                    gameModes[match.Results.GameMode]++;
                }
                else gameModes.Add(match.Results.GameMode, 1);
                if (maps.ContainsKey(match.Results.Map))
                {
                    maps[match.Results.Map]++;
                }
                else maps.Add(match.Results.Map, 1);
            }

            var orderMaps = maps.OrderByDescending(a => a.Value).Select(a => a.Key).Take(5);
            var orderModes = gameModes.OrderByDescending(a => a.Value).Select(a => a.Key).Take(5);
            
            var getResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/stats", null, MethodType.GET);
            var getFullServerStats = JsonConvert.DeserializeObject<FullServerStatsDto>(getResponse.JsonString);

            Assert.AreEqual(getResponse.StatusCode, "OK");
            Assert.IsNull(getResponse.ErrorMessage);
            Assert.NotNull(getFullServerStats);
            Assert.AreEqual(getFullServerStats.TotalMatchesPlayed, 2);
            Assert.AreEqual(getFullServerStats.AveragePopulation, averagePopulation);
            Assert.AreEqual(getFullServerStats.MaximumPopulation, maximumPopulation);
            Assert.AreEqual(getFullServerStats.MaximumMatchesPerDay, 2);
            Assert.AreEqual(getFullServerStats.AverageMatchesPerDay, 2);
            Assert.AreEqual(getFullServerStats.Top5GameModes, orderModes);
            Assert.AreEqual(getFullServerStats.Top5Maps, orderMaps);
        }
    }
}
