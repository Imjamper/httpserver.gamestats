using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Attributes;
using Kontur.GameStats.Server.Dto;
using Kontur.GameStats.Server.DTO;
using Newtonsoft.Json;
using static System.Int32;

namespace HttpClient
{
    public static class RandomGenerator
    {
        private static readonly Random Random = new Random();

        private static readonly List<string> Modes = new List<string>
        {
            "DM",
            "TDM",
            "SINGLEMATCH",
            "USUAL",
            "RATINGMATCH",
            "TOP",
            "HORDE"
        };

        private static readonly List<string> Maps = new List<string>
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

        private static readonly List<string> Players = new List<string>
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

        public static ServerDto GetServer()
        {
            var server = new ServerDto();
            server.Endpoint = $"localhost-{GetRandomInt()}";
            var serverInfo = new ServerInfoDto();
            serverInfo.GameModes.AddRange(GetRandomModes());
            server.Info = serverInfo;
            return server;
        }

        public static MatchDto GetMatch(string server)
        {
            var match = new MatchDto();
            match.Server = server;
            match.TimeStamp = DateTime.UtcNow;
            var matchInfo = new MatchResultDto();
            matchInfo.FragLimit = Random.Next(50);
            matchInfo.GameMode = GetRandomMode();
            matchInfo.Map = GetRandomMap();
            matchInfo.TimeElapsed = Random.Next(60) + Random.NextDouble();
            matchInfo.TimeLimit = Random.Next(20, 60);
            var count = Random.Next(Players.Count);
            for (int i = 0; i < count; i++)
            {
                 matchInfo.ScoreBoard.Add(GetRandomPlayerScore());   
            }
            match.Results = matchInfo;

            return match;
        }

        public static int GetRandomInt()
        {
            return Random.Next(MaxValue);
        }

        public static List<string> GetRandomModes()
        {
            return Modes.Take(Random.Next(Modes.Count)).ToList();
        }

        public static string GetRandomMode()
        {
            return Modes.ElementAt(Random.Next(Modes.Count));
        }

        public static string GetRandomMap()
        {
            return Maps.ElementAt(Random.Next(Modes.Count));
        }

        public static PlayerScoreDto GetRandomPlayerScore()
        {
            var playerScore = new PlayerScoreDto();
            playerScore.Deaths = Random.Next(30);
            playerScore.Frags = Random.Next(60);
            playerScore.Kills = Random.Next(25);
            playerScore.Name = Players.ElementAt(Random.Next(Players.Count));

            return playerScore;
        }
    }
}
