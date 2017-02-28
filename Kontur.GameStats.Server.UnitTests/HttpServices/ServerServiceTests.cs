using System.Collections.Generic;
using System.Linq;
using GL.HttpServer.Enums;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    [TestFixture]
    public class ServerServiceTests : ServiceTests
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
            var putResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/info", GameServer.Info, MethodType.PUT);

            Assert.AreEqual(putResponse.StatusCode, "OK");
            Assert.IsNull(putResponse.ErrorMessage);

            var match = PutMatchInfo();

            var getResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{match.TimeStamp.UtcDateTime:yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'}", null, MethodType.GET);
            var getMatchInfo = JsonConvert.DeserializeObject<MatchResultDto>(getResponse.JsonString);

            Assert.AreEqual(getResponse.StatusCode, "OK");
            Assert.IsNull(getResponse.ErrorMessage);
            Assert.NotNull(getMatchInfo);
            Assert.AreEqual(match.Results.ScoreBoard.Count, getMatchInfo.ScoreBoard.Count);
            Assert.AreEqual(match.Results.TimeLimit, getMatchInfo.TimeLimit);
        }

        [Test, Order(3)]
        public void GetAllServersInfo_PutServersInfo_GetSameServersInfos()
        {
            var testServerOne = GetServer("localhost-8111", "TestServer8111");
            var testServerTwo = GetServer("localhost-8222", "TestServer8222");

            var putResponse1 = ExecuteUrl($"servers/{testServerOne.Endpoint}/info", testServerOne.Info, MethodType.PUT);

            Assert.AreEqual(putResponse1.StatusCode, "OK");
            Assert.IsNull(putResponse1.ErrorMessage);

            var putResponse2 = ExecuteUrl($"servers/{testServerTwo.Endpoint}/info", testServerTwo.Info, MethodType.PUT);

            Assert.AreEqual(putResponse2.StatusCode, "OK");
            Assert.IsNull(putResponse2.ErrorMessage);

            var getResponse = ExecuteUrl("servers/info", null, MethodType.GET);
            var getServerInfos = JsonConvert.DeserializeObject<List<ServerDto>>(getResponse.JsonString);

            Assert.AreEqual(getResponse.StatusCode, "OK");
            Assert.IsNull(getResponse.ErrorMessage);
            Assert.NotNull(getServerInfos);
            Assert.IsTrue(getServerInfos.Any(a => a.Endpoint == testServerOne.Endpoint && a.Info.Name == testServerOne.Info.Name));
            Assert.IsTrue(getServerInfos.Any(a => a.Endpoint == testServerTwo.Endpoint && a.Info.Name == testServerTwo.Info.Name));
        }

        [Test, Order(4)]
        public void GetServerStats_PutMatches_GetValidStats()
        {
            var serverForStats = GetServer("localhost-6767", "TestServerForStats");
            var matches = PutMatchesInfo(3, serverForStats);

            var averagePopulation = matches.Sum(a => a.Results.ScoreBoard.Count) / (double)3;
            var maximumPopulation = matches.Max(a => a.Results.ScoreBoard.Count);
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
            
            var getResponse = ExecuteUrl($"servers/{serverForStats.Endpoint}/stats", null, MethodType.GET);
            var getFullServerStats = JsonConvert.DeserializeObject<FullServerStatsDto>(getResponse.JsonString);

            Assert.AreEqual(getResponse.StatusCode, "OK");
            Assert.IsNull(getResponse.ErrorMessage);
            Assert.NotNull(getFullServerStats);
            Assert.AreEqual(getFullServerStats.TotalMatchesPlayed, matches.Count);
            Assert.AreEqual(getFullServerStats.AveragePopulation, averagePopulation);
            Assert.AreEqual(getFullServerStats.MaximumPopulation, maximumPopulation);
            Assert.AreEqual(getFullServerStats.MaximumMatchesPerDay, matches.Count);
            Assert.AreEqual(getFullServerStats.AverageMatchesPerDay, matches.Count);
            Assert.AreEqual(getFullServerStats.Top5GameModes, orderModes);
            Assert.AreEqual(getFullServerStats.Top5Maps, orderMaps);
        }

        [Test, Order(5)]
        public void PutMatchInfo_PutMatchNoAdvertised_ReturnNotFound()
        {   
            var match = GetMatch();
            var noAdvertiseServer = GetServer("localhost-9999", "NoAdvertisedServer");
            var date = match.TimeStamp.UtcDateTime.ToString(UtcFormat);
            var putMatchResponse = ExecuteUrl($"servers/{noAdvertiseServer.Endpoint}/matches/{date}", match.Results, MethodType.PUT);
            Assert.AreEqual(putMatchResponse.StatusCode, "NotFound");
        }
    }
}
