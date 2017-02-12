using System.Collections.Generic;
using GL.HttpServer.Context;

namespace Kontur.GameStats.Server.DTO
{
    public class MatchInfo : JsonResponse
    {
        public MatchInfo()
        {
            ScoreBoard = new List<PlayerScore>();
        }

        public string Map { get; set; }
        public string GameMode { get; set; }
        public int FragLimit { get; set; }
        public int TimeLimit { get; set; }
        public double TimeElapsed { get; set; }
        public List<PlayerScore> ScoreBoard { get; set; }
    }
}