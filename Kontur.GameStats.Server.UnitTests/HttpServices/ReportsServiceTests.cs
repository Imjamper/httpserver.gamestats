using System;
using System.Collections.Generic;
using GL.HttpServer.Enums;
using Newtonsoft.Json;
using NUnit.Framework;
using HttpClient;
using Kontur.GameStats.Server.Dto;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    [TestFixture]
    public class ReportsServiceTests : ServiceTest
    {
        [Test, Order(1)]
        public void RecentMatches_PutNewMatches_GetSameMatchesInfo()
        {
            var match1 = RandomGenerator.GetMatch();
            match1.Server = GameServer.Endpoint;
            if (match1.TimeStamp != null)
            {
                var date1 = match1.TimeStamp.Value.ToString(UtcFormat);
                var putMatchResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date1}", match1.Results, MethodType.PUT);

                Assert.AreEqual(putMatchResponse.StatusCode, "OK");
                Assert.IsNull(putMatchResponse.ErrorMessage);
            }

            var match2 = RandomGenerator.GetMatch();
            match2.Server = GameServer.Endpoint;
            if (match2.TimeStamp != null)
            {
                var date2 = match2.TimeStamp.Value.ToUniversalTime().ToString(UtcFormat);
                var putMatch1Response = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date2}", match2.Results, MethodType.PUT);

                Assert.AreEqual(putMatch1Response.StatusCode, "OK");
                Assert.IsNull(putMatch1Response.ErrorMessage);
            }
            var matches = new List<MatchDto> { match2, match1 };
            var matchesJson = JsonConvert.SerializeObject(matches);
            var getResponse = ExecuteUrl("reports/recent-matches[/2]", null, MethodType.GET);

            var getRecentMatches = JsonConvert.DeserializeObject<List<MatchDto>>(getResponse.JsonString);

            Assert.AreEqual(getRecentMatches.Count, 2);
            Assert.AreEqual(getResponse.JsonString, matchesJson);
        }

        [Test, Order(2)]
        public void GetBestPlayers_PutNewMatches_GetPopularServers()
        {   
            var match1 = RandomGenerator.GetMatch();
            match1.Server = GameServer.Endpoint;
            if (match1.TimeStamp != null)
            {
                var date1 = match1.TimeStamp.Value.ToString(UtcFormat);
                var putMatchResponse = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date1}", match1.Results, MethodType.PUT);

                Assert.AreEqual(putMatchResponse.StatusCode, "OK");
                Assert.IsNull(putMatchResponse.ErrorMessage);
            }

            var match2 = RandomGenerator.GetMatch();
            match2.Server = GameServer.Endpoint;
            if (match2.TimeStamp != null)
            {
                var date2 = match2.TimeStamp.Value.ToString(UtcFormat);
                var putMatch1Response = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date2}", match2.Results, MethodType.PUT);

                Assert.AreEqual(putMatch1Response.StatusCode, "OK");
                Assert.IsNull(putMatch1Response.ErrorMessage);
            }

            var match3 = RandomGenerator.GetMatch();
            match3.Server = GameServer.Endpoint;
            if (match3.TimeStamp != null)
            {
                var date3 = match3.TimeStamp.Value.ToString(UtcFormat);
                var putMatch2Response = ExecuteUrl($"servers/{GameServer.Endpoint}/matches/{date3}", match3.Results, MethodType.PUT);

                Assert.AreEqual(putMatch2Response.StatusCode, "OK");
                Assert.IsNull(putMatch2Response.ErrorMessage);
            }

            var getResponse = ExecuteUrl("reports/best-players[/2]", null, MethodType.GET);

            var getRecentMatches = JsonConvert.DeserializeObject<List<ShortPlayerStatsDto>>(getResponse.JsonString);

            Assert.AreEqual(getRecentMatches.Count, 2);
        }
    }
}
