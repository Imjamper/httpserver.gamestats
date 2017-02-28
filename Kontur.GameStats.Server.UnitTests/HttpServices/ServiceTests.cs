using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using GL.HttpServer.Enums;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Kontur.GameStats.Server.UnitTests.TestModels;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Kontur.GameStats.Server.UnitTests.HttpServices
{
    public class ServiceTests
    {
        public const string UtcFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'";
        
        private ServerDto _serverInfo;

        public ServerDto GameServer => _serverInfo ?? (_serverInfo = GetServer());

        public List<MatchDto> PutMatchesInfo(int count = 5, ServerDto server = null, bool advertise = false)
        {
            var matches = new List<MatchDto>();
            if (server == null)
            {
                server = GameServer;
            }
            if (advertise)
            {
                var advertisePut = ExecuteUrl($"servers/{server.Endpoint}/info", server.Info, MethodType.PUT);
                Assert.AreEqual(advertisePut.StatusCode, "OK");
                Assert.IsNull(advertisePut.ErrorMessage);
            }
            for (int i = 0; i < count; i++)
            {
                var match = GetMatch();
                match.Server = server.Endpoint;
                var date = match.TimeStamp.UtcDateTime.ToString(UtcFormat);
                var putMatchResponse = ExecuteUrl($"servers/{server.Endpoint}/matches/{date}", match.Results, MethodType.PUT);
                Assert.AreEqual(putMatchResponse.StatusCode, "OK");
                Assert.IsNull(putMatchResponse.ErrorMessage);

                matches.Add(match);
            }
            return matches;
        }

        public MatchDto PutMatchInfo(ServerDto server = null, bool advertise = false)
        {
            return PutMatchesInfo(1, server, advertise).FirstOrDefault();
        }

        public ClientResponse ExecuteUrl(string path, object json, MethodType methodType)
        {
            var url = $"http://localhost:{TestsServerStarter.Port}/{path}";
            using (var client = new HttpClient())
            {
                HttpContent httpContent = null;
                if (json != null)
                    httpContent = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
                var response = new ClientResponse();
                try
                {
                    switch (methodType)
                    {
                        case MethodType.PUT:
                            using (
                                var r =
                                    client.PutAsync(new Uri(url, UriKind.Absolute), httpContent)
                                        .GetAwaiter()
                                        .GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                response.JsonString = result;
                                response.StatusCode = r.StatusCode.ToString();
                                break;
                            }
                        case MethodType.GET:
                            using (var r = client.GetAsync(new Uri(url, UriKind.Absolute)).GetAwaiter().GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                response.JsonString = result;
                                response.StatusCode = r.StatusCode.ToString();
                                break;
                            }
                        case MethodType.POST:
                            using (
                                var r =
                                    client.PostAsync(new Uri(url, UriKind.Absolute), httpContent)
                                        .GetAwaiter()
                                        .GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                response.JsonString = result;
                                response.StatusCode = r.StatusCode.ToString();
                                break;
                            }
                        case MethodType.DELETE:
                            using (var r = client.DeleteAsync(new Uri(url, UriKind.Absolute)).GetAwaiter().GetResult())
                            {
                                string result = r.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                response.JsonString = result;
                                response.StatusCode = r.StatusCode.ToString();
                                break;
                            }
                        default:
                            response.StatusCode = "888";
                            break;
                    }
                }
                catch (HttpRequestException)
                {
                    response.ErrorMessage = "Server is shout down";
                    response.StatusCode = "888";
                    return response;
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = ex.Message;
                    response.StatusCode = "888";
                    return response;
                }
                return response;
            }
        }

        #region Generator

        private readonly Random _random = new Random();

        private readonly List<string> _modes = new List<string>
        {
            "DM",
            "TDM",
            "SINGLEMATCH",
            "USUAL",
            "RATINGMATCH",
            "TOP",
            "HORDE"
        };

        private readonly List<string> _maps = new List<string>
        {
            "DM-HelloWorld",
            "DM-1on1-Rose",
            "DM-Kitchen",
            "DM-Camper Paradise",
            "DM-Appalachian Wonderland",
            "DM-ServerStats",
            "DM-SomeGame",
            "DM-YoungPeople",
            "DM-Dota2",
            "DM-PerfectWorld"
        };

        private readonly List<string> Players = new List<string>
        {
            "Player1",
            "Player2",
            "Player3",
            "Player4",
            "Player5",
            "Player6",
            "Player7",
            "Player8",
            "Player9",
            "Player10"
        };

        public ServerDto GetServer(string endpoint = null, string serverName = null)
        {
            var server = new ServerDto();
            server.Endpoint = endpoint ?? "localhost-7777";
            var serverInfo = new ServerInfoDto();
            serverInfo.Name = serverName ?? "TestGameServer";
            serverInfo.GameModes.AddRange(GetRandomModes());
            server.Info = serverInfo;
            return server;
        }

        public MatchDto GetMatch()
        {
            var match = new MatchDto();
            match.TimeStamp = DateTimeOffset.UtcNow;
            var matchInfo = new MatchResultDto();
            matchInfo.FragLimit = _random.Next(50);
            matchInfo.GameMode = GetRandomMode();
            matchInfo.Map = GetRandomMap();
            matchInfo.TimeElapsed = _random.Next(60) + _random.NextDouble();
            matchInfo.TimeLimit = _random.Next(20, 60);
            var count = _random.Next(Players.Count);
            for (int i = 0; i < count; i++)
            {
                matchInfo.ScoreBoard.Add(GetRandomPlayerScore());
            }
            match.Results = matchInfo;

            return match;
        }

        public int GetRandomInt()
        {
            return _random.Next(Int32.MaxValue);
        }

        public List<string> GetRandomModes()
        {
            return _modes.Take(_random.Next(_modes.Count)).ToList();
        }

        public string GetRandomMode()
        {
            return _modes.ElementAt(_random.Next(_modes.Count));
        }

        public string GetRandomMap()
        {
            return _maps.ElementAt(_random.Next(_modes.Count));
        }

        public PlayerScoreDto GetRandomPlayerScore()
        {
            var playerScore = new PlayerScoreDto();
            playerScore.Deaths = _random.Next(30);
            playerScore.Frags = _random.Next(60);
            playerScore.Kills = _random.Next(25);
            playerScore.Name = Players.ElementAt(_random.Next(Players.Count));

            return playerScore;
        }

        #endregion
    }
}
