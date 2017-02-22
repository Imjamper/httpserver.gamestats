using System.Collections.Generic;
using GL.HttpServer.Attributes;
using GL.HttpServer.Entities;
using Kontur.GameStats.Server.Dto;

namespace Kontur.GameStats.Server.Entities
{
    [MapsTo(typeof(MatchResultDto))]
    public class MatchResult : Entity
    {
        public MatchResult()
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
